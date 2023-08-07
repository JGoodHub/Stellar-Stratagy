Shader "Unlit/RingShader"
{
    Properties
    {
        _CircleColor("Circle Color", color) = (1, 1, 1, 1)
        _GlowColor("Glow Color", color) = (1, 1, 1, 1)

        _GlowInnerRadius("Glow Inner Radius", Range(0, 0.5)) = 0.5

        _CircleInnerRadius("Circle Inner Radius", Range(0, 0.5)) = 0.5
        _CircleThickness("Circle Thickness", Range(0, 0.5)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
        LOD 100

        ZWrite Off
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

            float4 _CircleColor;
            float4 _GlowColor;

            float _GlowInnerRadius;

            float _CircleInnerRadius;
            float _CircleThickness;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f v) : SV_Target
            {
                const float distToCentre = length(float2(0.5, 0.5) - v.uv);

                // Outside the outer circle radius
                if (distToCentre > _CircleInnerRadius + _CircleThickness)
                    return fixed4(0, 0, 0, 0);

                // Inside the inner glow radius
                if (_GlowInnerRadius < _CircleInnerRadius && distToCentre < _CircleInnerRadius)
                {
                    if (distToCentre < _GlowInnerRadius)
                        return fixed4(0, 0, 0, 0);

                    float glowAlpha = (distToCentre - _GlowInnerRadius) / (_CircleInnerRadius - _GlowInnerRadius);
                    return fixed4(_GlowColor.r, _GlowColor.g, _GlowColor.b, _GlowColor.a * glowAlpha);
                }

                if (distToCentre < _CircleInnerRadius)
                    return fixed4(0, 0, 0, 0);

                return _CircleColor;
            }
            ENDCG
        }
    }
}