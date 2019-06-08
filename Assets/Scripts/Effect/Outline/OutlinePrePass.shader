Shader "ApcShader/OutlinePrePass" {
	SubShader {
		Pass {
			CGPROGRAM

			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag

			fixed4 _OutlineCol;

			float4 vert(float4 vertex : POSITION) : SV_POSITION {
				return UnityObjectToClipPos(vertex);
			}

			fixed4 frag(float4 pos : SV_POSITION) : SV_Target {
				return _OutlineCol;
			}
			ENDCG
		}
	}
}