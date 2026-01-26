Shader "LucasShaders/S_ProceduralSkyBox"
{
    Properties
    {
        _SkyColor("_Sky Color", Color) = (1,1,1,1)
        _HorizonColor("Horizon Color", Color) = (1,1,1,1)
        _BottomColor("Ground Color", Color) = (1,1,1,1)
        _blendSkyStrength("Blending Sky", float) = 0.1
        _blendGroundStrength("Blending Ground", float) = 0.1
        _powerStrength("Power Strength", float) = 1
        
        _CellSize ("Cell Size", Range(800,5000)) = 800
        _skyStrength("Sky Strength", float) = 0.1
        _NoiseSpeed("Noise Speed", float) = 0.1
        _CloudContrast("Cloud Contrast", float) = 1.5
        _CloudLayers("Cloud Layers", float) = 5
    }

    SubShader
    {
        Tags
        {"RenderType"="Background" "Queue"="Background" "RenderPipeline"="UniversalPipeline" "PreviewType"="Skybox"}

        Pass
        {
            Name "SkyboxGradient"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Only required include
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _SkyColor;
                half4 _HorizonColor;
                half4 _BottomColor;
                half _blendSkyStrength;
                half _blendGroundStrength;
                half _powerStrength;
            CBUFFER_END
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                float3 posWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionWS = posWS;
                return OUT;
            }

            half4 WorldGradient(float3 dir)
            {
                half sky = Smootherstep(0, _blendSkyStrength, dir.y);
                half ground = Smootherstep(0, _blendGroundStrength, -dir.y);

                half horizon = 1 - (sky + ground);
                horizon = pow(saturate(horizon), _powerStrength);

                return  _SkyColor * sky + _HorizonColor * horizon + _BottomColor * ground;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 d = normalize(IN.positionWS);
                return saturate(WorldGradient(d));
            }
            ENDHLSL
        }

        Pass
        {
            Name "Clouds"
            Blend SrcAlpha OneMinusSrcAlpha   // transparency blending
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            float _CellSize;
            float _skyStrength;
            float _NoiseSpeed;
            float _CloudContrast;
            int _CloudLayers;

            struct Attributes
            {
                float4 vertex : POSITION;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            // ---------------- Utility ----------------

            float3 rand3dTo3d(float3 p)
            {
                p = float3(
                    dot(p, float3(127.1, 311.7, 74.7)),
                    dot(p, float3(269.5, 183.3, 246.1)),
                    dot(p, float3(113.5, 271.9, 124.6))
                );
                return frac(sin(p) * 43758.5453);
            }

            float easeInOut(float t) { return lerp(t*t, 1-(1-t)*(1-t), t); }

            float perlinNoise(float3 value)
            {
                float3 fraction = frac(value);
                float ix = easeInOut(fraction.x);
                float iy = easeInOut(fraction.y);
                float iz = easeInOut(fraction.z);

                float noiseZ[2];
                for (int z = 0; z <= 1; z++)
                {
                    float noiseY[2];
  
                    for (int y = 0; y <= 1; y++)
                    {
                        float noiseX[2];
      
                        for (int x = 0; x <= 1; x++)
                        {
                            float3 cell = floor(value) + float3(x, y, z);
                            float3 gradient = rand3dTo3d(cell) * 2 - 1;
                            float3 diff = fraction - float3(x, y, z);
                            noiseX[x] = dot(gradient, diff);
                        }
                        noiseY[y] = lerp(noiseX[0], noiseX[1], ix);
                    }
                    noiseZ[z] = lerp(noiseY[0], noiseY[1], iy);
                }

                return lerp(noiseZ[0], noiseZ[1], iz);
            }

            float fbm(float3 p)
            {
                float sum = 0;
                float amp = 0.5;
                float freq = 1.0;
                for (int i = 0; i < _CloudLayers; i++)
                {
                    sum += perlinNoise(p * freq) * amp;
                    freq *= 2.0;
                    amp *= 0.5;
                }

                return sum;
            }

            // ---------------- Vertex / Fragment ----------------

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                // animate noise in X direction
                float3 animatedPos = IN.worldPos / (_CellSize*2) + float3(_Time.x * _NoiseSpeed, 0, 0);

                // multi-octave FBM
                float noise = fbm(animatedPos);

                // normalize to 0..1
                noise = saturate(noise * 0.5 + 0.5);

                // make clouds softer/fluffy
                noise = pow(noise, _CloudContrast);

                // mask by height to fade near horizon
                float mask = smoothstep(0, _skyStrength, normalize(IN.worldPos).y);

                return half4(1,1,1, noise * mask);
            }

            ENDHLSL
        }
    }
}
