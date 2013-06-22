float4x4 matWorldViewProj;
float4x4 matWorld;

float4 vecLightDir;
float4 vecEye;

struct VS_OUTPUT
{
   float4 Pos : POSITION;
   float3 Light : TEXCOORD0;
   float3 Norm : TEXCOORD1;
   float3 View : TEXCOORD2;
};

VS_OUTPUT VS(float4 Pos : POSITION, float3 Normal : NORMAL)
{
   VS_OUTPUT Out = (VS_OUTPUT)0;
   Out.Pos = mul(Pos, matWorldViewProj); // transform Position
   Out.Light = vecLightDir; // L
   float3 PosWorld = normalize(mul(Pos, matWorld));
   Out.View = vecEye - PosWorld; // V
   Out.Norm = mul(Normal, matWorld); // N
   return Out;
}

float4 PS(float3 Light:TEXCOORD0, float3 Norm:TEXCOORD1,float3 View:TEXCOORD2):COLOR
{
   float4 diffuse = { 1.0f, 0.0f, 0.0f, 1.0f};
   float4 ambient = { 0.1f, 0.0f, 0.0f, 1.0f};
   float3 Normal = normalize(Norm);
   float3 LightDir = normalize(Light);
   float3 ViewDir = normalize(View);
   float4 diff = saturate(dot(Normal, LightDir)); // diffuse component
   // R = 2 * (N.L) * N – L
   float3 Reflect = normalize(2 * diff * Normal - LightDir);
   float4 specular = pow(saturate(dot(Reflect, ViewDir)), 8); // R.V^n
   // I = Acolor + Dcolor * N.L + (R.V)n
   return ambient+diffuse*diff+specular;
}
technique RenderizarPhong
{
   pass Pass_0
   {
      VertexShader = compile vs_2_0 VS();
      PixelShader = compile ps_2_0 PS();
   }
}