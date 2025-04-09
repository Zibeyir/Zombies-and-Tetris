Shader "Custom/URPStencilMask"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+1" }
        Pass
        {
            Name "Mask"
            Tags { "LightMode"="UniversalForward" }

            Stencil
            {
                Ref 1
                Comp always
                Pass replace
            }

            ZWrite Off
            Blend Zero One

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return half4(0,0,0,0); // görsənməsin
            }
            ENDHLSL
        }
    }
}
