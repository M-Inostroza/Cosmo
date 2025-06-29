Shader "Custom/DrillBrushFeather"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BrushPos ("Brush Position", Vector) = (0,0,0,0)
        _BrushSize ("Brush Size", Float) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _BrushPos;
            float _BrushSize;

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
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float dist = distance(i.uv, _BrushPos.xy);
                
                // Feather the alpha: inside radius = 0 alpha, outside = 1
                float alphaMask = smoothstep(_BrushSize * 0.5, _BrushSize, dist);

                float4 tex = tex2D(_MainTex, i.uv);
                tex.a *= alphaMask; // reduce alpha inside circle
                return tex;
            }
            ENDCG
        }
    }
}
