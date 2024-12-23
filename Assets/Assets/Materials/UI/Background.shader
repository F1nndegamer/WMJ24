Shader "Custom/MeshGradientBackground"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 1, 0, 1)
        _Color3 ("Color 3", Color) = (0, 0, 1, 1)
        _Color4 ("Color 4", Color) = (1, 0, 0, 1)
        _Color5 ("Color 5", Color) = (0, 1, 0, 1)
        _Color6 ("Color 6", Color) = (0, 0, 1, 1)
        _BlurRadius ("Blur Radius", Range(0, 10)) = 0.5
        _ShapeSize ("Shape Size", Range(0.1, 10)) = 0.5
        _TimeMultiplier ("Animation Speed", Range(0.1, 10)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

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
            float4 _MainTex_ST;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;
            float4 _Color5;
            float4 _Color6;
            float _BlurRadius;
            float _ShapeSize;
            float _TimeMultiplier;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float random(float2 st)
            {
                return frac(sin(dot(st.xy, float2(12.9898,78.233))) * 43758.5453123);
            }

            float gradientShape(float2 uv, float2 center, float size)
            {
                float dist = length(uv - center);
                return smoothstep(size, size - _BlurRadius, dist);
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float time = _TimeMultiplier * _Time.y;

                // Randomized centers for shapes
                float2 center1 = float2(0, 0.5 + 0.3 * cos(time * 1.1));
                float2 center2 = float2(0.03, 0.8 + 0.1 * cos(time * 0.7));
                float2 center3 = float2(0.02, 0.2 + 0.4 * cos(time * 0.9));
                float2 center4 = float2(1+0, 0.5 + 0.9 * cos(time * 1.1));
                float2 center5 = float2(1-0.03, 0.8 + 0 * cos(time * 0.7));
                float2 center6 = float2(1-0.02, 0.2 + 0.5 * cos(time * 0.9));

                // Shape gradients
                float shape1 = gradientShape(uv, center1, _ShapeSize * 0.5);
                float shape2 = gradientShape(uv, center2, _ShapeSize * 0.7);
                float shape3 = gradientShape(uv, center3, _ShapeSize * 0.6);
                float shape4 = gradientShape(uv, center4, _ShapeSize * 0.4);
                float shape5 = gradientShape(uv, center5, _ShapeSize * 0.6);
                float shape6 = gradientShape(uv, center6, _ShapeSize * 0.8);

                // Combine gradients
                float4 color = _Color1 * shape1 + _Color2 * shape2 + _Color3 * shape3 + _Color4 * shape4 + _Color5 * shape5 + _Color6 * shape6;

                return color;
            }
            ENDCG
        }
    }
}
