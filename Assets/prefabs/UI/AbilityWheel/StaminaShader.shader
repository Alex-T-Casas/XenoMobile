Shader "Unlit/StaminaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Progress ("Progress", range(0,1)) = 0.5
        _ProgressMaskTex("Texture", 2D) = "white" {}
        _ColorProg("Prpgress Color", color) = (1,1,1,1)
        _ColorFull("Prpgress Full Color", color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        Zwrite off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uvOrig : TEXTCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _ProgressMaskTex;
            float4 _MainTex_ST;
            float _Progress;
            float4 _ColorProg;
            float4 _ColorFull;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvOrig = v.uv; // sets the origin of the objects uv to the v
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 progressMaskTex = tex2D(_ProgressMaskTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                float2 coord = i.uvOrig - float2(1,0.5); // creates the cord of the origin at the mid right of object

                float2 dir = normalize(coord); // sets the dir to be equal to the normalized cords form the uv origin
                float2 downDir = float2(0, -1); // estabishes that the down dir starts at the bottom of v.uv
                float angle = degrees(acos(dot(dir, downDir))); // angle is calulated by geting the acos dotproduct of dir and down dir and change it from rad to deg
                float rangeMin = (180 - 45) / 2; // angle range min (67.5 deg)
                float rangeMax = (180 - 45) / 2 + 45; // angle range max (112.5 deg)

                float progressMask = progressMaskTex.x;

                if (angle > rangeMin && angle < rangeMax) 
                {
                    float normalizedRange = (angle - rangeMin) / 45; // the normalised range is a progress 
                    if (_Progress < normalizedRange && normalizedRange != 0)
                    {
                        progressMask = 0;
                    }

                }

                if (progressMask != 0)
                {
                    float4 progressCol = _ColorProg;
                    if (_Progress == 1)
                    {
                        progressCol = _ColorFull;
                    }
                    col = progressCol;
                }

                return col;//float4(progressMask, progressMask, progressMask, 1);
            }
            ENDCG
        }
    }
}
