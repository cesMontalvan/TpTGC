//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj :WORLDVIEWPROJECTION; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))
float4x4 matGiro; //matriz para rotar al modelo en el eje x
//float4x4 matGiroAux=matGiro;
//float angulo;

float3 dirLuz={0.0f,0.0f,-1.0f};
float4 normalTransformada={0.0f,0.0f,0.0f,0.0f};
//float4 posTemporal={0.0f,0.0f,0.0f,0.0f};

//Input del Vertex Shader
struct VS_INPUT 
{
   float4 Position : POSITION0;
   float3 Normal :   NORMAL;
   float4 Color :    COLOR0;
   float2 Texcoord : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT 
{
   float4 Position : POSITION0;
   float4 Color :    COLOR0;
   float2 Texcoord : TEXCOORD0;
   
};



//Vertex Shader
VS_OUTPUT vs_main(VS_INPUT Input)
{
   VS_OUTPUT Output=(VS_OUTPUT) 0;

   //transformando la normal al mismo sistema de coordenadas
   float4 normalTransformada = mul(Input.Normal,matWorldViewProj);

   //seteando el color
   Output.Color.rgba = 1.0f;
   Output.Color *= dot(normalTransformada,dirLuz);

   //Proyectar posicion
   //float4 posTemporal=Input.Position;
   //posTemporal.x += cos(Input.Position +(angulo * 2.0f));

   //Output.Position = mul( posTemmporal, matWorldViewProj);
   //matWorldViewProj=
   //Input.Position.x += cos(angulo)*2.0f;
   
   Output.Position = mul( Input.Position, matWorldViewProj);
   
   //Propago las coordenadas de textura
   Output.Texcoord = Input.Texcoord;

   //Propago el color x vertice
   //Output.Color = Input.Color;
   
   //Devolviendo la normal
   //Output.Normal = Input.Normal;

   return( Output );
   
}
//funcion 2
VS_OUTPUT



//Pixel shader




technique Renderizar
{
   pass Pass_0
   {
          //CullMode=None;
	  VertexShader = compile vs_2_0 vs_main();
	  PixelShader = NULL;//compile ps_2_0 ps_main();
   }

}

//technique Rotar
//{
//   pass Pass_0
//   {
//      VertexShader = compile vs_2_0 vs_main2();
//      PixelShader = NULL;
//   }
//}
