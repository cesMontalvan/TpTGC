//Matrices de transformacion 
float4x4 matWorld;                               //Matriz de transformacion World
float4x4 matWorldView;                           //Matriz World * View
float4x4 matWorldViewProj :WORLDVIEWPROJECTION;  //Matriz World * View * Projection
float4x4 matInverseTransposeWorld;                //Matriz Transpose(Invert(World))

texture textura_Base1;
sampler2D muestra1=sampler_state
{
   Texture= <textura_Base1>;
   MINFILTER=LINEAR;
   MAGFILTER=LINEAR;
   MIPFILTER=LINEAR;
};

float4 matAmbiente;
float4 luzAmbiente;
float4 matDiffuse;
float4 luzDifusa;
float4 matEspecular;
float4 luzEspecular;
float4 vecDirLuz;
float4 posOjo;
float exponencial;

//Input del Vertex Shader
struct VS_INPUT 
{
   float4 Position : POSITION0;
   float3 Normal :   NORMAL0;
   float4 Color :    COLOR0;
   float2 Texcoord : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT 
{
   float4 Position : POSITION0;
   float3 normal : TEXCOORD0;
   float3 posW : TEXCOORD1;
};

VS_OUTPUT vertexShaderPhong (VS_INPUT input1)
{
   VS_OUTPUT output1 = (VS_OUTPUT) 0 ;

   //transformando la normal al espacio homogeneo
   output1.normal=mul(float4(input1.Normal,0.0f),matInverseTransposeWorld).xyz;
   output1.normal=normalize(output1.normal);

   //transformando la pos del vertice
   output1.posW=mul(input1.Position,matWorld);//aca mepa que es al reves es .Position

   //transformando no se que
   output1.Position=mul(input1.Position,matWorldViewProj);

   //output1.Texcoord=input1.Texcoord;

   return output1;
};
  
 
float4 pixelShader (float3 normalW: TEXCOORD0, float3 posW:TEXCOORD1):COLOR
{
   normalW=normalize(normalW);
   float3 toEye=normalize(posOjo.xyz-posW);
   float3 r=reflect(-vecDirLuz.xyz,normalW);
   float t=pow(max(dot(r,toEye),0.0f),exponencial);
   float s=max(dot(vecDirLuz.xyz,normalW),0.0f);

   float3 spec=t*(matEspecular*luzEspecular).rgb;
   float3 difus=s*(matDiffuse*luzDifusa).rgb;
   float3 ambient=(matAmbiente*luzAmbiente).rgb;
   
   return float4(spec+difus+ambient,matDiffuse.a); 
}

technique RenderizarPhong
{
   pass Pass_0
   {
      VertexShader = compile vs_2_0 vertexShaderPhong();
      PixelShader = compile ps_2_0 pixelShader();
   }
}
