//Shader "Custom/DecolorizeShader"
//{
//    Properties
//    {
//        _MainTex ("Texture", 2D) = "white" {}
//        _Intensity ("Intensity", Range(0, 1)) = 0
//    }
//    SubShader
//    {
//        // No culling or depth
//        Cull Off ZWrite Off ZTest Always
//
//        Pass
//        {
//            CGPROGRAM
//            #pragma vertex vert_img
//            #pragma fragment frag
//
//            #include "UnityCG.cginc"
//
//            struct v2f
//            {
//                float2 uv : TEXCOORD0;
//            };
//            
//
//            sampler2D _MainTex;
//            float _Intensity;
//
//            fixed4 frag (v2f i) : SV_Target
//            {
//                float4 col1 = tex2D(_MainTex, i.uv);
//                float bw = col1.r*.3 + col1.g*.59 + col1.b*.11;
//
//                float4 col2 = fixed4(bw, bw, bw, col1.a);
//
//                return lerp(col1, col2, _Intensity);
//            }
//            ENDCG
//        }
//    }
//}
Shader "Hidden/BWDiffuse" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _bwBlend ("Black & White blend", Range (0, 1)) = 0
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            uniform sampler2D _MainTex;
            uniform float _bwBlend;
            float4 frag(v2f_img i) : COLOR {
                float4 c = tex2D(_MainTex, i.uv);
                
                float lum = c.r*.3 + c.g*.59 + c.b*.11;
                float3 bw = float3( lum, lum, lum ); 
                
                float4 result = c;
                result.rgb = lerp(c.rgb, bw, _bwBlend);
                return result;
            }
            ENDCG
        }
    }
}