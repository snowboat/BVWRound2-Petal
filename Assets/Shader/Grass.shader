Shader "Custom/Grass" {
	Properties{
		_MainTex("Grass Texture", 2D) = "white" {}
		_TimeScale("Time Scale", float) = 0.2
		_Offset("Wave Offset", Vector) = (0, 0, 0, 0)
		_Direction("Wind Direction", Vector) = (0, 0, 0, 0)
	}

	SubShader{
		Tags{ "RenderType" = "Opaque" }
		Pass{
		Tags{ "LightMode" = "ForwardBase" }

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		CGPROGRAM
#pragma vertex vert  
#pragma fragment frag  
#include "UnityCG.cginc" 
#include "Lighting.cginc"

		sampler2D _MainTex;
		half _TimeScale;
		float4 _Direction;
		float4 _Offset;

		struct a2v {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
		};

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			fixed3 normal : TExCOORD1;
			fixed3 worldPos : TEXCOORD2;
		};


		v2f vert(a2v v) {
			v2f o;
			float4 offset = float4(0,0,0,0);
			float amount = sin(_TimeScale * 3.1415 * _Time.y) * clamp(v.vertex.y, 0, 1);
			offset = _Direction * amount + _Offset;
			o.pos = UnityObjectToClipPos(v.vertex + offset);
			o.uv = v.texcoord.xy;
			o.normal = UnityObjectToWorldNormal(v.normal);
			o.worldPos = mul(unity_ObjectToWorld, v.vertex + offset).xyz;
			return o;
		}

		fixed4 frag(v2f i) : SV_Target {
			fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
			fixed3 worldLight = normalize(UnityWorldSpaceLightDir(i.worldPos));
			fixed3 albedo = tex2D(_MainTex, i.uv).rgb;
			fixed3 normal = normalize(i.normal);
			fixed3 diffuse = _LightColor0.rgb * albedo.rgb * saturate(dot(normal, worldLight));
			fixed3 color = ambient + diffuse;
			return fixed4(color, 1.0);
		}

			ENDCG
		}
	}
	FallBack Off
}