Shader "Unlit/DissolveShader"
{
	Properties
	{
		[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
		_Threshold("Threshold", Range(0.0, 1.0)) = 0
		_Color("Color", Color) = (1, 1, 1, 1)
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				fixed _Threshold;
				fixed4 _Color;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				fixed2 random(fixed2 st)
				{
					st = fixed2(
						dot(st,fixed2(127.1,311.7)),
						dot(st,fixed2(269.5,183.3))
					);
					return -1.0 + 2.0*frac(sin(st)*43758.5453123);
				}

				float perlinNoise(fixed2 st)
				{
					fixed2 p = floor(st);
					fixed2 f = frac(st);
					fixed2 u = f * f*(3.0 - 2.0*f);

					float v00 = random(p + fixed2(0,0));
					float v10 = random(p + fixed2(1,0));
					float v01 = random(p + fixed2(0,1));
					float v11 = random(p + fixed2(1,1));

					return lerp(lerp(dot(v00, f - fixed2(0,0)), dot(v10, f - fixed2(1,0)), u.x),
								 lerp(dot(v01, f - fixed2(0,1)), dot(v11, f - fixed2(1,1)), u.x),
								 u.y) + 0.5f;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 color = tex2D(_MainTex, i.uv);
					float noise = smoothstep(_Threshold, _Threshold + 0.1, perlinNoise(i.uv * 20));
					_Color.a = noise;
					fixed4 color2 = lerp(
						color,
						_Color,
						1 - step(1, noise)
					);

					return lerp(
						color,
						color2,
						1 - step(_Threshold, 0)
					);
				}
				ENDCG        }
    }
}
