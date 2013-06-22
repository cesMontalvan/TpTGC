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

float4 vecDirLuz={-150,1000,0,0};
float4 luzAmbiente;
float4 luzDifusa;
float4 luzEspecular;
float matAmbiente;
float matDiffuse;
float matEspecular;
float4 posOjo={0,0,-10,0};//-100
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
   float4 ColorDifuso : COLOR1;
   float4 ColorEspecular : COLOR0;
   float2 Texcoord : TEXCOORD0;
};

struct VS_OUTPUT1 
{
   float4 Position : POSITION0;
   float4 ColorDifuso : COLOR0;
   float4 ColorEspecular : COLOR1;
   //float3 ColorAmbiente : TEXCOORD1;
   float2 Texcoord : TEXCOORD;
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
   float3 ambient= matAmbiente*luzAmbiente.xyz;
   //float3 diffuse= saturate(dot(normal,light))*matDiffuse; //calculando la difusa
   float s=max(dot(vecDirLuz,normal),0.0f);
   float3 diffuse= s*luzDifusa.rgb*matDiffuse; 
   //float3 specular= pow(saturate(dot(normal,halfway)),exponencial)*matEspecular;
   float3 specular= pow(saturate(dot(normal,halfway)),exponencial)*matEspecular;
   float2 texcoord=IN.texCoord;
   float4 texColor=tex2D(muestra1,texcoord);
   //float3 color=(saturate(ambient+diffuse)*texColor+specular)+luzDifusa.rgb; 
   float3 base={0.5,0.5,0.5};
   float3 color=(saturate(ambient+diffuse)*texColor+specular)*luzDifusa+0.15*texColor;//

   float alpha=matDiffuse*texColor.a;//float alpha=matDiffuse.a*texColor.a;
   //return float4(diffuse,alpha); //usado para probar solamente la difusa
   return float4(color,alpha);
   //return float4(specular,alpha);//usado para probar solamente la especular
}
VS_OUTPUT vertexShader (VS_INPUT input1)
{
   VS_OUTPUT output1 = (VS_OUTPUT) 0 ;
   //Seteando la posicion
   output1.Position=mul(input1.Position,matWorldViewProj);
   output1.ColorDifuso=input1.Color;
   output1.Texcoord=input1.Texcoord;
   return output1;
};

VS_OUTPUT vertexShaderAmbiente (VS_INPUT input1)
{
   VS_OUTPUT output1 = (VS_OUTPUT) 0 ;
   //Seteando la posicion
   output1.Position=mul(input1.Position,matWorldViewProj);
   output1.ColorDifuso=luzAmbiente*matAmbiente;
   output1.Texcoord=input1.Texcoord;
   return output1;
};

VS_OUTPUT vertexShaderDifusa (VS_INPUT input1)
{
   VS_OUTPUT output1 = (VS_OUTPUT) 0 ;
   //transformando la normal al espacio homogeneo
   float4 normalTransf=mul(float4 (input1.Normal,0.0f),matWorldViewProj);
   normalTransf=normalize(normalTransf);
   //maximo(para no tomar negativos)---producto escalar de la luz difusa y la normal segun la formula de calculo de 
   //cantidad de luz difusa que refleja el vertice=max(vectorDirLuz.vectorNormal,0).(VecLuzDifusaxVecDiffuseMaterial)
   //mepa que VecLuzDifusa y VecDiffuseMaterial dicen cuanta luz absorve y reflejan 
   float s=max(dot(vecDirLuz,normalTransf),0.0f);
   output1.ColorDifuso.rgb=saturate(s*luzDifusa.rgb)*matDiffuse;
   output1.ColorDifuso.a=matDiffuse;
   //Seteando la posicion
   output1.Position=mul(input1.Position,matWorldViewProj);
   //output1.ColorDifuso=input1.Color;
   output1.Texcoord=input1.Texcoord;
   return output1;
};

VS_OUTPUT vertexShaderDifusayAmbiente(VS_INPUT input1)
{
   VS_OUTPUT output1 = (VS_OUTPUT) 0 ;
   //transformando la normal al espacio homogeneo
   float4 normalTransf=mul(float4 (input1.Normal,0.0f),matWorldViewProj);
   normalTransf=normalize(normalTransf);
   //maximo(para no tomar negativos)---producto escalar de la luz difusa y la normal segun la formula de calculo de 
   //cantidad de luz difusa que refleja el vertice=max(vectorDirLuz.vectorNormal,0).(VecLuzDifusaxVecDiffuseMaterial)
   //mepa que VecLuzDifusa y VecDiffuseMaterial dicen cuanta luz absorve y reflejan 
   float s=max(dot(vecDirLuz,normalTransf),0.0f);
   float3 difusoAux = saturate(s*luzDifusa.rgb)*matDiffuse;
   float3 ambienteAux= luzAmbiente*matAmbiente;
   output1.ColorDifuso.rgb=difusoAux+ambienteAux;
   output1.ColorDifuso.a=matDiffuse;
   //Seteando la posicion
   output1.Position=mul(input1.Position,matWorldViewProj);
   output1.Texcoord=input1.Texcoord;
   return output1;
}

VS_OUTPUT1 vertexShaderEspecularDifusaAmbiente (VS_INPUT input1)
{
   VS_OUTPUT1 output1 = (VS_OUTPUT1) 0 ;

   //Seteando la posicion
   output1.Position=mul(input1.Position,matWorldViewProj);

   output1.Texcoord=input1.Texcoord;

   //transformando la normal al espacio homogeneo
   float4 normalTransf=mul(float4 (input1.Normal,0.0f),matWorldViewProj);
   normalTransf=normalize(normalTransf);

   //pasando la pos del vertice al espacio homogeneo
   float3 posVertice= mul(input1.Position,matWorldViewProj).xyz;
   
   //calculando el vector que va desde el vertice que refleja a el ojo del observador/Camara
    float3 posOjoTransf= normalize(posVertice-posOjo.xyz);
  
   vecDirLuz=normalize(vecDirLuz);

   //calculando el vector refleccion (rayo de luz que sale despues de rebotar en el vertice)
   float3 ref= reflect(vecDirLuz,normalTransf);
   
   //determinando cuanta luz especular va al ojo
   float t=pow(max(dot(ref,posOjoTransf),0.0f),exponencial);

   //calculando la intensidad de luz difusa que el vertice rebota
   float s=max(dot(vecDirLuz,normalTransf),0.0f);

   //calculando las luces: especular, difusa, y ambiente
   float3 spec=t*luzEspecular.rgb*matEspecular;

   float3 difusa=saturate(s*luzDifusa.rgb)*matDiffuse;
   float3 ambiente=matAmbiente*luzAmbiente.rgb;

   //float3 ambienteMasDifusa=saturate(ambiente+difusa);
   output1.ColorDifuso= float4 (difusa,0);//(ambienteMasDifusa,1);
   output1.ColorAmbiente=ambiente;
   output1.ColorDifuso.a=matDiffuse;
   output1.ColorEspecular=float4 (spec,1);

   return output1; 
}

float4 pixelShader (float4 c: COLOR0,float2 tex0:TEXCOORD):COLOR  
{
   float3 texColor=tex2D (muestra1,tex0).rgb;
   return float4 (texColor,c.a);
}

float4 pixelShaderSoloAmbiente (float4 c: COLOR0,float2 tex0:TEXCOORD):COLOR  
{
   float3 texColor=tex2D (muestra1,tex0).rgb;
   float3 difuso=c.rgb*0.2+0.8*texColor;
   return float4 (difuso.rgb,c.a);
}

float4 pixelShaderAmbiente (float4 c: COLOR0,float2 tex0:TEXCOORD):COLOR  
{
   float3 texColor=tex2D (muestra1,tex0).rgb;
   float3 difuso=c.rgb*texColor;
   return float4(difuso,c.a);
}


float4 pixelShaderEspecular (float4 especul:COLOR0,float4 dif: COLOR1,float2 tex0:TEXCOORD,float3 ambiente:TEXCOORD1):COLOR 
{
   float3 texColor=tex2D (muestra1,tex0).rgb;
   //float3 difuso=saturate((ambiente+dif)*texColor)*dif.rgb;//saturate(texColor*c);
   return dif;//float4 (difuso,1);
}
technique RenderizarTextura
{
   pass Pass_0
      {
         VertexShader = compile vs_2_0 vertexShader();
         PixelShader =NULL;// compile ps_2_0 pixelShader();
      }
}

technique RenderizarAmbiente
{
   pass Pass_0
      {
         VertexShader = compile vs_2_0 vertexShaderAmbiente();
         PixelShader = compile ps_2_0 pixelShaderSoloAmbiente();
      }
}

technique RenderizarDifusa
{
   pass Pass_0
   {
      VertexShader = compile vs_2_0 vertexShaderDifusa();
      PixelShader = compile ps_2_0 pixelShaderAmbiente();
   }
}
technique RenderizarDifusaYAmbiente
{
   pass Pass_0
      {
         VertexShader = compile vs_2_0 vertexShaderDifusayAmbiente();
         PixelShader = compile ps_2_0 pixelShaderAmbiente();
      }
}

technique RenderizarEspecularDifusaAmbiente
{
   pass Pass_0
   {
      VertexShader=compile vs_2_0 vertexShaderEspecularDifusaAmbiente();
      PixelShader = NULL;//compile ps_2_0 pixelShaderEspecular();
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