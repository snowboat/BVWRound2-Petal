Shader "Custom/Grass" {
	Properties{
		_MainTex("Grass Texture", 2D) = "white" {}
		_TimeScale("Time Scale", float) = 0.2
		_Direction("Wind Direction", Vector) = (0, 0, 0, 0)
	}

	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Opaque" "IgnoreProject" = "True" }
		Pass{
		Tags{ "LightMode" = "ForwardBase" }

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		CGPROGRAM
#pragma vertex vert  
#pragma fragment frag  
#include "UnityCG.cginc"   

		sampler2D _MainTex;
		half _TimeScale;
		float4 _Direction;

		struct a2v {
			float4 vertex : POSITION;
			float4 texcoord : TEXCOORD0;
		};

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
		};


		v2f vert(a2v v) {
			v2f o;
			float4 offset = float4(0,0,0,0);
			float amount = sin(3.1415 * _Time.y * clamp(v.vertex.y, 0, 1))  * _TimeScale;
			offset = _Direction * amount;
			o.pos = UnityObjectToClipPos(v.vertex + offset);
			o.uv = v.texcoord.xy;
			return o;
		}

		fixed4 frag(v2f i) : SV_Target{
			return tex2D(_MainTex, i.uv);
		}

			ENDCG
		}
	}
	FallBack Off
}