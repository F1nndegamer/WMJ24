Shader "Custom/UIBlur_URP"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _BlurRadius ("Blur Radius", Range(0, 64)) = 4
        _OverlayColor ("Overlay Color", Color) = (0.5, 0.5, 0.5, 1)
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Transparent" }
        Pass
        {
            Name "BlurPass"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BlurRadius;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 texelSize = float2(1.0 / _ScreenParams.x, 1.0 / _ScreenParams.y);
                float4 color = float4(0, 0, 0, 0);

                // Apply a simple Gaussian blur
                for (int x = -2; x <= 2; x++)
                {
                    for (int y = -2; y <= 2; y++)
                    {
                        float weight = 1.0 - length(float2(x, y)) / _BlurRadius;
                        color += tex2D(_MainTex, IN.uv + texelSize * float2(x, y)) * weight;
                    }
                }

                return color / 25.0; // Normalize the result
            }
            ENDHLSL
        }
    }
}
