//Matrices de transformacion 
float4x4 matWorld;                                      //Matriz de transformacion World
float4x4 matWorldView;                                  //Matriz World * View
float4x4 matWorldViewProj :WORLDVIEWPROJECTION;         //Matriz World * View * Projection
float4x4 matInverseTransposeWorld;                      //Matriz Transpose(Invert(World))

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
float4 dirOjo;
float exponencial;

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
   float4 ColorDifuso : COLOR0;
   //float4 ColorEspecular : COLOR1;
   float2 Texcoord : TEXCOORD0;
};


struct VS_OUTPUT2
{
   float4 position: POSITION;
   float2 texCoord: TEXCOORD0;
   float3 normal: TEXCOORD1;
   float3 view: TEXCOORD2;
};


VS_OUTPUT2 vsBlinnPhong(VS_INPUT IN)
{
   VS_OUTPUT2 OUT;
   OUT.position=mul(IN.Position,matWorldViewProj);
   OUT.texCoord=IN.Texcoord;
   
   //leí que en vez de usar la inversa transpuesta había que usarla pero sin la 4ºcolumna pero ni siquiera así funciono,
   //tampoco funcionaba con la multiplicacion por la inversa transpuesta completa la unica con la que funcionó es con la
   // matWorldViewProj, sino la especular no quedaba fija sino que parecía una textura
   float4x3 matWorldIT = {matInverseTransposeWorld._m00,matInverseTransposeWorld._m01,matInverseTransposeWorld._m02                                                       ,matInverseTransposeWorld._m10,matInverseTransposeWorld._m11,matInverseTransposeWorld._m12
                         ,matInverseTransposeWorld._m20,matInverseTransposeWorld._m21,matInverseTransposeWorld._m22
                         ,matInverseTransposeWorld._m30,matInverseTransposeWorld._m31,matInverseTransposeWorld._m32};
   OUT.normal=mul(IN.Normal,matWorldViewProj);//acá es donde iba lo comentado arriba

   float4 posOjo2=mul(posOjo,matWorldViewProj);
   float4 worldPos=mul(IN.Position,matWorldViewProj);//algo parecido a lo de la normal acá supestamente llevaba el pto al matWorld

   OUT.view= posOjo.xyz-worldPos.xyz;
   
   return OUT;
}

float4 psBlinnPhong(VS_OUTPUT2 IN):COLOR
{
   float3 light=normalize(-vecDirLuz.xyz);
   
   float3 view=normalize(IN.view);
   float3 normal=normalize(IN.normal);
   float3 halfway=normalize(-light+view);

   float3 ambient= (luzAmbiente*matAmbiente).xyz;
   //float3 diffuse= saturate(dot(normal,light))*matDiffuse.rgb; //calculando la difusa

   float s=max(dot(vecDirLuz,normal),0.0f);
   float3 diffuse= s*(matDiffuse*luzDifusa).rgb; //saturate lo cagaba, lo hacía ver todo negro
   //float3 specular= pow(saturate(dot(normal,halfway)),exponencial)*matEspecular;
   float3 specular= pow(saturate(dot(normal,halfway)),exponencial)*matEspecular;

   float2 texcoord=IN.texCoord;
   float4 texColor=tex2D(muestra1,texcoord);

   //float3 color=(saturate(ambient+diffuse)*texColor+specular)+luzDifusa.rgb; 
   float3 base={0.5,0.5,0.5};
  
   float3 color=(saturate(ambient+diffuse)*texColor+specular)*luzDifusa; 
   float alpha=matDiffuse.a*texColor.a;

   //return float4(diffuse,alpha); usado para probar solamente la difusa
   return float4(color,alpha);
   //return float4(specular,alpha);usado para probar solamente la especular
}

VS_OUTPUT vertexShaderDifusa (VS_INPUT input1)
{
   VS_OUTPUT output1 = (VS_OUTPUT) 0 ;
   //transformando la normal al espacio homogeneo
   float4 normalTransf=mul(float4 (input1.Normal,0.0f),matInverseTransposeWorld);
   normalTransf=normalize(normalTransf);
   //maximo(para no tomar negativos)---producto escalar de la luz difusa y la normal segun la formula de calculo de 
   //cantidad de luz difusa que refleja el vertice=max(vectorDirLuz.vectorNormal,0).(VecLuzDifusaxVecDiffuseMaterial)
   //mepa que VecLuzDifusa y VecDiffuseMaterial dicen cuanta luz absorve y reflejan y sus componentes van de 0 a 1 son%?
   float s=max(dot(vecDirLuz,normalTransf),0.0f);
   output1.ColorDifuso.rgb=s*(matDiffuse*luzDifusa).rgb;
   output1.ColorDifuso.a=matDiffuse.a;
   //Seteando la posicion
   output1.Position=mul(input1.Position,matWorldViewProj);
   //output1.ColorDifuso=input1.Color;
   output1.Texcoord=input1.Texcoord;
   return output1;
};
VS_OUTPUT vertexShaderMasAmbient(VS_INPUT input1)
{
   VS_OUTPUT output1 = (VS_OUTPUT) 0 ;
   //transformando la normal al espacio homogeneo
   float4 normalTransf=mul(float4 (input1.Normal,0.0f),matWorldViewProj);
   normalTransf=normalize(normalTransf);
   //maximo(para no tomar negativos)---producto escalar de la luz difusa y la normal segun la formula de calculo de 
   //cantidad de luz difusa que refleja el vertice=max(vectorDirLuz.vectorNormal,0).(VecLuzDifusaxVecDiffuseMaterial)
   //mepa que VecLuzDifusa y VecDiffuseMaterial dicen cuanta luz absorve y reflejan y sus componentes van de 0 a 1 son%?
   float s=max(dot(vecDirLuz,normalTransf),0.0f);
   float3 difusoAux = s*(matDiffuse*luzDifusa).rgb;
   float3 ambienteAux= (luzAmbiente*matAmbiente).rgb;
   output1.ColorDifuso.rgb=difusoAux+ambienteAux;
   output1.ColorDifuso.a=matDiffuse.a;
   //Seteando la posicion
   output1.Position=mul(input1.Position,matWorldViewProj);
   output1.Texcoord=input1.Texcoord;
   return output1;
}

VS_OUTPUT vertexShaderDifuseAmbienteSpecular(VS_INPUT input1)
{
   VS_OUTPUT output1 = (VS_OUTPUT) 0 ;

   //transformando la normal al espacio homogeneo
   float4 normalTransf=mul(float4 (input1.Normal,0.0f),matWorldViewProj);
   normalTransf=normalize(normalTransf);

   //pasando la pos del vertice al espacio homogeneo
   float3 posVertice= mul(input1.Position,matWorld).xyz;
   //por que a posOjo no le hacen matWorld???
   posOjo=mul(posOjo,matWorld);

   
   //calculando el vector que va desde el vertice que refleja a el ojo del observador/Camara
   float3 posOjoTransf= normalize(posVertice-posOjo.xyz);//
   
   //porque a vecDirLuz no le hacen matWorld???
   vecDirLuz= mul(vecDirLuz,matWorld);
   vecDirLuz=normalize(vecDirLuz);

   //calculando el vector refleccion (rayo de luz que sale despues de rebotar en el vertice)
   float3 ref= reflect(vecDirLuz,normalTransf);
   
   //determinando cuanta luz especular va al ojo
   float t=pow(max(dot(ref,posOjoTransf),0.0f),exponencial);

   //calculando la intensidad de luz difusa que el vertice rebota
   float s=max(dot(vecDirLuz,normalTransf),0.0f);

   //calculando las luces: especular, difusa, y ambiente
   float3 spec=t*(matEspecular*luzEspecular).rgb;

   float3 difusa=s*(matDiffuse*luzDifusa).rgb;
   float3 ambiente=(luzAmbiente*matAmbiente).rgb;
   
   output1.ColorDifuso.rgb= ambiente+difusa+spec;
   output1.ColorDifuso.a=matDiffuse.a;
   
   //Seteando la posicion
   output1.Position=mul(input1.Position,matWorldViewProj);

   output1.Texcoord=input1.Texcoord;

   return output1; 
}
  

 
float4 pixelShader (float4 c: COLOR0,float2 tex0:TEXCOORD,float4 spec:COLOR1):COLOR  
{
   float3 texColor=tex2D (muestra1,tex0).rgb;
   float3 difuso=c.rgb*texColor;
   return float4 (difuso+spec.rgb,c.a);
}
technique RenderizarDifusa
{
   pass Pass_0
   {
      VertexShader = compile vs_2_0 vertexShaderDifusa();
      PixelShader = compile ps_2_0 pixelShader();
   }
}
technique RenderizarDifusaYAmbient
{
   pass Pass_0
      {
         VertexShader = compile vs_2_0 vertexShaderMasAmbient();
         PixelShader = compile ps_2_0 pixelShader();
      }
}
technique RenderizarLuzCompleta
{
   pass Pass_0
   {
      VertexShader=compile vs_2_0 vertexShaderDifuseAmbienteSpecular();
      PixelShader = compile ps_2_0 pixelShader();
   }
}
technique BlinnPhong
{
   pass Pass_0
   {
      VertexShader=compile vs_2_0 vsBlinnPhong();
      PixelShader =compile ps_2_0 psBlinnPhong();
   }
}  