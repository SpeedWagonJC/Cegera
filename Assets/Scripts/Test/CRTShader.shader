Shader "Custom/CRT"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScanlineIntensity ("Scanline Intensity", Range(0,1)) = 0.3
        _Curve ("Curve", Range(0,0.2)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _ScanlineIntensity;
            float _Curve;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // Curvatura CRT
                float2 centered = o.uv - 0.5;
                centered *= 1 + dot(centered, centered) * _Curve;
                o.uv = centered + 0.5;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Scanlines
                float scan = sin(i.uv.y * 800) * _ScanlineIntensity;
                col.rgb -= scan;

                return col;
            }
            ENDCG
        }
    }
}
