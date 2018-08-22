/*
HLSL packing rules are similar to performing a #pragma pack 4 with Visual Studio, which packs data into 4-byte boundaries. Additionally, 
HLSL packs data so that it does not cross a 16-byte boundary. Variables are packed into a given four-component vector until the variable 
will straddle a 4-vector boundary; the next variables will be bounced to the next four-component vector.
https://docs.microsoft.com/en-us/windows/desktop/direct3dhlsl/dx-graphics-hlsl-packing-rules
*/
/************* DATA STRUCTS **************/
cbuffer data :register(b0)
{
	float4x4 World;
	float4x4 WorldInverseTranspose;
	float4x4 ViewInverse;
	float4x4 WorldViewProj;
	float4 LightDirection;

    float3 SurfaceColor;
    float Kd;
    float3 LampColor;
    float SpecExpon;
    float3 AmbiColor;
    float Kr;
};


/* data from application vertex buffer */
struct VS_IN {
	float3 Position	: POSITION;
    float3 Normal : NORMAL;
    float3 Tangent : TANGENT;
    float3 Binormal : BINORMAL;
	float2 UV		: TEXCOORD;
};

/* data passed from vertex shader to pixel shader */
struct PS_IN {
	float4 HPosition	: SV_POSITION;
	float2 UV		: TEXCOORD0;
	// The following values are passed in "World" coordinates since
	//   it tends to be the most flexible and easy for handling
	//   reflections, sky lighting, and other "global" effects.
	float3 LightVec	: TEXCOORD1;
	float3 WorldNormal	: NORMAL;
    float3 WorldTangent : TANGENT;
    float3 WorldBinormal : BINORMAL;
	float3 WorldView	: TEXCOORD2;
};


/*********** Generic Vertex Shader ******/

PS_IN VS(VS_IN IN) {
	
	PS_IN OUT = (PS_IN)0;

    //OUT.WorldNormal = mul(World, float4(IN.Normal, 0)).xyz;
    //OUT.WorldTangent = mul(World, float4(IN.Tangent, 0)).xyz;
    //OUT.WorldBinormal = mul(World, float4(IN.Binormal, 0)).xyz;


    OUT.WorldNormal = mul(IN.Normal, WorldInverseTranspose).xyz;
    OUT.WorldTangent = mul(IN.Tangent, WorldInverseTranspose).xyz;
    OUT.WorldBinormal = mul(IN.Binormal, WorldInverseTranspose).xyz;

	
	float4 Po = float4(IN.Position, 1);
	
	float4 Pw = mul(Po, World);	// convert to "world" space
    //float4 Pw = mul(World, Po); // convert to "world" space

	float4 Lw = LightDirection;

	if (Lw.w == 0) {
		OUT.LightVec = -normalize(Lw.xyz);
	}
	else {
		// we are still passing a (non-normalized) vector
		OUT.LightVec = Lw.xyz - Pw.xyz;
	}

	OUT.UV = IN.UV.xy;

	OUT.WorldView = normalize(ViewInverse[3].xyz - Pw.xyz);
	
	//OUT.HPosition = mul(Po, WorldViewProj);
    OUT.HPosition = mul(WorldViewProj, Po);
	
	return OUT;
}



/********* pixel shaders ********/



void metal_shared(PS_IN IN,
	uniform float Kd,
	uniform float SpecExpon,
	uniform float Kr,
	//uniform samplerCUBE EnvSampler,
	float3 LightColor,
	uniform float3 AmbiColor,
	out float3 DiffuseContrib,
	out float3 SpecularContrib)
{
    float3 Ln = normalize(IN.LightVec.xyz);
    float3 Nn = normalize(IN.WorldNormal);
    float3 Vn = normalize(IN.WorldView);
    float3 Hn = normalize(Vn + Ln);
    float4 litV = lit(dot(Ln, Nn), dot(Hn, Nn), SpecExpon);
    DiffuseContrib = litV.y * Kd * LightColor + AmbiColor;
    SpecularContrib = litV.z * LightColor;
    float3 reflVect = -reflect(Vn, Nn);
    //float3 ReflectionContrib = Kr * texCUBE(EnvSampler, reflVect).rgb;
    //SpecularContrib += ReflectionContrib;
	
}

float4 PS(PS_IN IN) : SV_Target
{
    float3 diffContrib;
    float3 specContrib;
    metal_shared(IN, Kd, SpecExpon, Kr
	/*,EnvSampler*/
	, LampColor, AmbiColor,
	diffContrib, specContrib);
    float3 result = SurfaceColor * (specContrib + diffContrib);
    return saturate(float4(result, 1));
}

