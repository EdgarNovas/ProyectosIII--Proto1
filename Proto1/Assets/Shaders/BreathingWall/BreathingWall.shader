Shader "Custom/BreathingPulsingWall"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Color1 ("Base Color", Color) = (0.6, 0, 0, 1)
        _Color2 ("Pulse Color", Color) = (1, 0.2, 0.2, 1)
        _EmissionStrength ("Emission Strength", Range(0,5)) = 2
        _PulseSpeed ("Pulse Speed", Range(0,5)) = 1
        _BreathAmplitude ("Breathing Amplitude", Range(0, 0.2)) = 0.05
        _BreathFrequency ("Breathing Frequency", Range(0, 3)) = 1
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color1, _Color2;
            float _EmissionStrength, _PulseSpeed;
            float _BreathAmplitude, _BreathFrequency;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
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

                // --- Breathing vertex displacement ---
                // Use world position and time to offset vertices smoothly.
                float breath = sin(_Time.y * _BreathFrequency) * _BreathAmplitude;

                // Move vertices along their normals for organic expansion/contraction
                float3 displacedVertex = v.vertex.xyz + v.normal * breath;

                // Transform to clip space
                o.vertex = UnityObjectToClipPos(float4(displacedVertex, 1.0));

                // Pass UVs through
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Pulse over time
                float pulse = (sin(_Time.y * _PulseSpeed) * 0.5 + 0.5);

                // Sample texture
                fixed4 tex = tex2D(_MainTex, i.uv);

                // Mix texture color with pulse colors (keep texture detail)
                fixed3 pulseColor = lerp(_Color1.rgb, _Color2.rgb, pulse);
                fixed3 baseColor = tex.rgb * pulseColor;   // <-- this is the key line

                // Add emission on top
                fixed3 emission = baseColor * pulse * _EmissionStrength;

                return fixed4(baseColor + emission, tex.a);
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
