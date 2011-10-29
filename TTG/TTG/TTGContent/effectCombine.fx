sampler BloomSampler : register(s0);
sampler BaseSampler : register(s1);

float BloomIntensity;
float BaseIntensity;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 bloom = tex2D(BloomSampler, texCoord);
    float4 base = tex2D(BaseSampler, texCoord);
    
    bloom = bloom * BloomIntensity;
    base = base * BaseIntensity;
    
    base *= (1 - saturate(bloom)); // prevent being overbright
    
    return base + bloom;
}


technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
