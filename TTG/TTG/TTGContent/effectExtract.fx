sampler TextureSampler : register(s0);

float Threshold;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 c = tex2D(TextureSampler, texCoord);

    return float4(smoothstep(Threshold, 1.0, c.rgb), 1);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
