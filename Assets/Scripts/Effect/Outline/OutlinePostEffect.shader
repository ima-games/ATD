Shader "Custom/OutLinePostEffect" {

	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_BlurTex("Blur", 2D) = "white"{}
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	sampler2D _MainTex;
	float4 _MainTex_TexelSize;
	sampler2D _BlurTex;
	float4 _BlurTex_TexelSize;
	float4 _Offset;
	float _OutlineStrength;
	
	struct v2f_blur {
		float4 pos : SV_POSITION;
		float4 uv  : TEXCOORD0;
	};

	struct v2f_cull {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f_add {
		float4 pos : SV_POSITION;
		float2 uv  : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
	};

	//高斯模糊 vert shader
	v2f_blur vert_blur(appdata_img v) {
		v2f_blur o;
		_Offset *= _MainTex_TexelSize.xyxy;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xyxy + _Offset.xyxy * float4(1, 1, -1, -1) * 3.0;
		return o;
	}

	//高斯模糊 pixel shader
	fixed4 frag_blur(v2f_blur i) : SV_Target {
		fixed4 color = 0.5 * (tex2D(_MainTex, i.uv.xy) + tex2D(_MainTex, i.uv.zw));
		return color;
	}

	//Blur图和原图进行相减获得轮廓
	v2f_cull vert_cull(appdata_img v) {
		v2f_cull o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		//dx中纹理从左上角为初始坐标，需要反向
		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv.y = 1 - o.uv.y;
		#endif	
		return o;
	}

	fixed4 frag_cull(v2f_cull i) : SV_Target {
		fixed4 colorMain = tex2D(_MainTex, i.uv);
		fixed4 colorBlur = tex2D(_BlurTex, i.uv);
		//最后的颜色是_BlurTex - _MainTex，周围0-0=0，黑色；边框部分为描边颜色-0=描边颜色；中间部分为描边颜色-描边颜色=0。最终输出只有边框
		//return fixed4((colorBlur - colorMain).rgb, 1);
		return colorBlur - colorMain;
	}

	v2f_add vert_add(appdata_img v) {
		v2f_add o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv1.xy = o.uv.xy = v.texcoord.xy;
		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv.y = 1 - o.uv.y;
		#endif	
		return o;
	}

	fixed4 frag_add(v2f_add i) : SV_Target {
		fixed4 ori = tex2D(_MainTex, i.uv1);
		fixed4 blur = tex2D(_BlurTex, i.uv);
		fixed4 final = ori + blur * _OutlineStrength;
		return final;
	}

	ENDCG

	SubShader {

		//pass 0: 高斯模糊
		Pass {
			ZTest Off
			Cull Off
			ZWrite Off
			Fog{ Mode Off }

			CGPROGRAM
			#pragma vertex vert_blur
			#pragma fragment frag_blur
			ENDCG
		}
		
		//pass 1: 剔除中心部分 
		Pass {
			ZTest Off
			Cull Off
			ZWrite Off
			Fog{ Mode Off }

			CGPROGRAM
			#pragma vertex vert_cull
			#pragma fragment frag_cull
			ENDCG
		}

		//pass 2: 最终叠加
		Pass {
			ZTest Off
			Cull Off
			ZWrite Off
			Fog{ Mode Off }

			CGPROGRAM
			#pragma vertex vert_add
			#pragma fragment frag_add
			ENDCG
		}

	}
}
