using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using TgcViewer;
using TgcViewer.Example;
using TgcViewer.Utils.TgcGeometry;

using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;


namespace AlumnoEjemplos.I_SEE_DEAD_PIXELS
{
    public class Pared : Malla
    {
        public Effect efecto2;
        //public short[] verticesDelindex;
        Vector3 vecNormal;
        public Vector3 VecNormal
        {
            set { vecNormal = value; }
            get { return vecNormal; }
        }
        //public int espejado;
        //public TgcBoundingBox boundingBoxAsociado;
        public String funcion;
        //Mesh meshPared1 = null;
        //private VertexBuffer vertexBufferTemp;
        //short[] contenidoIndexBufer;
        public Texture textura = null;
        public Texture textura2 = null;
        public struct EstructVertice //estructura usada para almacenar c/u de los elementos levantados del Vertex Buffer
        {
            public Vector3 p;
            public Vector3 n;
            public float tu, tv;
            public static readonly VertexFormats Formato = VertexFormats.Position | VertexFormats.Normal | VertexFormats.Texture1;
        }
        //public void crearBoundingBoxAsociado(Vector3[] dirVertices)
        //{
            //Vector3[] dirVertices = null;
        //    Device dispositivo = GuiController.Instance.D3dDevice;
            //meshPared1 = this.malla1;
        //    //creando y declararando el tipo de la variable donde almacenaré el vertex buffer
        //    vertexBufferTemp = new VertexBuffer(typeof(EstructVertice), meshPared1.NumberVertices, dispositivo, Usage.None, EstructVertice.Formato, Pool.Default);
        //    //lockeando el vertex buffer, la manera de usar el vertex siempre tiene que ser 1º-lockearlo 2º-trabajar con el vertex buffer 3º-deslockearlo, entonces
        //    //el metodo nombreDeUnMesh.LockVertexBuffer me devuelve un array que lo voy a almacenar en un array de elementos tipo EstructVertice que declare antes
            //EstructVertice[] vertices = (EstructVertice[])meshPared1.LockVertexBuffer(typeof(EstructVertice), LockFlags.None, meshPared1.NumberVertices);
        //    //cargando los vertices en un array para pasarlo al TgcBoundingBox.ComputeFromPoints
            //dirVertices = new Vector3[vertices.Length];
            //for (int i = 0; i < vertices.Length; i++)
            //{
            //    dirVertices[i] = vertices[i].p;
            //}
            //deslockeando el buffer
            //meshPared1.UnlockVertexBuffer();
        //    this.boundingBoxAsociado = TgcBoundingBox.computeFromPoints(dirVertices);
            //this.boundingBoxAsociado.move(PosicionInicial);

        //    //probando para ver el index Buffer
        //    IndexBuffer buferIndice = this.malla1.IndexBuffer;
        //    short[] indicesBufer = (short[])buferIndice.Lock(0, typeof(short), LockFlags.None, 3 * this.malla1.NumberFaces); //(typeof(short),3*this.malla1.NumberFaces,LockFlags.None);
        //    contenidoIndexBufer = new short[indicesBufer.Length];
        //    for (int i = 0; i < indicesBufer.Length; i++)
        //    { contenidoIndexBufer[i] = indicesBufer[i]; }
        //    buferIndice.Unlock();
        //    this.verticesDelindex = contenidoIndexBufer;
        //
        //}
        //public Vector3 puntoMasCercano(Vector3 pto, Vector3[] puntos)
        //{
        //    Vector3 vert = pto - puntos[0];
        //    Vector3 vec1 = new Vector3(0, 0, 0);
        //    float norma;
        //    double distancia, distancia2 = 0;
        //    bool primeraVez = true;
        //
        //    for (int i = 0; i < puntos.Length; i++)
        //    {
        //        norma = (pto.X - puntos[i].X) * (pto.X - puntos[i].X) + (pto.Y - puntos[i].Y * pto.Y - puntos[i].Y) + (pto.Z - puntos[i].Z) * (pto.Z - puntos[i].Z);
        //        distancia = Math.Sqrt((double)norma);
        //        if (primeraVez)
        //        {
        //            distancia2 = distancia;
        //            vert = puntos[i];
        //            primeraVez = false;
        //        }
        //        if (distancia < distancia2)
        //        {
        //            vert = puntos[i];
        //            distancia2 = distancia;
        //        }
        //    }
        //    return vert;
        //}
        public void inicializar(Vector3 normal)
        {
            Device dispositivo = GuiController.Instance.D3dDevice;
            this.efecto2 = Effect.FromFile(dispositivo, GuiController.Instance.ExamplesMediaDir + "ModelosPropios\\shaderParedPrueba.fx", null, null, ShaderFlags.None, null);
            textura = TextureLoader.FromFile(dispositivo, GuiController.Instance.ExamplesMediaDir + this.RutaDeLaTextura);
            //textura2 = TextureLoader.FromFile(dispositivo, GuiController.Instance.ExamplesMediaDir + "ModelosPropios\\CS-Black-Texture.bmp");
            if (this.funcion == "piso" || this.funcion == "techo")
            {
                VecNormal = normal;
                this.inicializarGral();
            }
            if (this.funcion == "paredLarga")
            {
                VecNormal = normal;
                this.crearParedLarga();
                //this.crearPlano(orientado);
                //this.crearBoundingBoxAsociado(esquinas);
            }
            if (this.funcion == "paredCorta")
            {
                VecNormal = normal;
                this.crearParedCorta();
                //this.crearBoundingBoxAsociado(esquinas);
            }
            //if (this.funcion == "paredLargaIzquierda")
            //{
            //    VecNormal = normal;
            //    this.crearParedLargaIzq();
            //}
        }
        public float calcularCoordenada(float coordAprox)
        {
            double resto = 0;
            double truncacion = 0;
            double aux1 = 0;

            aux1 = coordAprox / 10;
            truncacion = Math.Truncate(aux1);
            resto = Math.Abs(aux1 - truncacion) * 10;

            if (resto < 2.6)
            {
                if (resto < 1.125) { return (float)(Math.Truncate(coordAprox / 10) * 10); }
                else { return (float)Math.Truncate(coordAprox / 10) * 10 + 2.5f * (Math.Sign(coordAprox)); }
            }
            else
            {
                if (resto < 5.1)
                {
                    if (resto - 2.5 < 1.125) { return (float)((Math.Truncate(coordAprox / 10) * 10) + 2.5 * (Math.Sign(coordAprox))); }
                    else { return (float)((Math.Truncate(coordAprox / 10) * 10) + 5.0 * (Math.Sign(coordAprox))); }
                }
                else
                {
                    if (resto < 7.6)
                    {
                        if (resto - 5 < 1.125) { return (float)((Math.Truncate(coordAprox / 10) * 10) + 5 * (Math.Sign(coordAprox))); }
                        else { return (float)((Math.Truncate(coordAprox / 10) * 10) + 7.5 * (Math.Sign(coordAprox))); }
                    }
                    else
                    {
                        if (resto - 7.5 < 1.125) { return (float)((Math.Truncate(coordAprox / 10) * 10) + 7.5 * (Math.Sign(coordAprox))); }
                    }
                }
            }
            return (float)((Math.Truncate(coordAprox / 10) * 10) + 10 * (Math.Sign(coordAprox)));
        }
        public void deformarse(Disparo disparo)
        {
            Device dispositivo = GuiController.Instance.D3dDevice;
            Vector3 normalInvertida, ptoColisionado;
            normalInvertida.X = this.VecNormal.X * -1;
            normalInvertida.Y = this.VecNormal.Y * -1;
            normalInvertida.Z = this.VecNormal.Z * -1;
            ptoColisionado.X = normalInvertida.X * (disparo.colisionador.radio) + disparo.colisionador.PosicionActual.X;
            ptoColisionado.Y = normalInvertida.Y * (disparo.colisionador.radio) + disparo.colisionador.PosicionActual.Y;
            ptoColisionado.Z = normalInvertida.Z * (disparo.colisionador.radio) + disparo.colisionador.PosicionActual.Z;
            #region abollandoParedLarga
            if (this.VecNormal.X != 0)//se trata de una pared larga
            {
                float coordExactaZ = calcularCoordenada(ptoColisionado.Z);
                float coordExactaY = calcularCoordenada(ptoColisionado.Y);
                float posRelativaAlMuroZ = -this.PosicionActual.Z + coordExactaZ;
                float posRelativaAlMuroY = -this.PosicionActual.Y + coordExactaY;

                short nivel = nivelDeImpacto(disparo);
                int profundidad = profDeImpacto(disparo);
                short direccion = dirDeImpacto(disparo);

                EstructVertice[] vertices = (EstructVertice[])this.malla1.LockVertexBuffer(typeof(EstructVertice), LockFlags.None, this.malla1.NumberVertices);
                float vertCentral = Math.Abs(posRelativaAlMuroY / 2.5f) * 337 + (posRelativaAlMuroZ / 2.5f); //se ubica el vertice central
                if (nivel == 0) { vertices[(int)(vertCentral)].p -= this.VecNormal * (0.4f * profundidad); }
                if (nivel == 1)
                {
                    switch (direccion)
                    {
                        case 1:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 337)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 336)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 337)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 336)].p -= this.VecNormal * (0.35f * profundidad);
                            break;
                        case 2:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 337)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 336)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 337)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 336)].p -= this.VecNormal * (0.3f * profundidad);
                            break;
                        case 3:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 337)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 336)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 337)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 336)].p -= this.VecNormal * (0.3f * profundidad);
                            break;
                        case 4:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 337)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 336)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 337)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 336)].p -= this.VecNormal * (0.35f * profundidad);
                            break;
                        case 5:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 337)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 336)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 337)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 336)].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 6:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 337)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 336)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral + 337)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 336)].p -= this.VecNormal * (0.3f * profundidad);
                            break;
                        case 7:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 337)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral - 336)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 337)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 336)].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 8:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 337)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral - 336)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 337)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral + 336)].p -= this.VecNormal * (0.35f * profundidad);
                            break;
                        default:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 337)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral - 336)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral + 337)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral + 336)].p -= this.VecNormal * (0.35f * profundidad);
                            break;
                    }

                }
                if (nivel == 2)
                {
                    vertices[(int)(vertCentral)].p -= this.VecNormal * (0.3f * profundidad);
                    for (int i = 1; i < 4; i++)
                    {
                        vertices[(int)(vertCentral) + i].p -= this.VecNormal * (0.3f * profundidad);
                        vertices[(int)(vertCentral) - i].p -= this.VecNormal * (0.3f * profundidad);
                    }
                    for (int n = 1; n < 4; n++)
                    {
                        vertices[(int)(vertCentral) + n * 337].p -= this.VecNormal * (0.3f * profundidad);
                        vertices[(int)(vertCentral) - n * 337].p -= this.VecNormal * (0.3f * profundidad);
                        for (int i = 1; i < 4; i++)
                        {
                            vertices[(int)(vertCentral) + n * 337 + i].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral) + n * 337 - i].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral) - n * 337 - i].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral) - n * 337 + i].p -= this.VecNormal * (0.3f * profundidad);
                        }
                    }
                    vertices[(int)(vertCentral) - 337 * 3 - 3].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) - 337 * 3 - 2].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) - 337 * 3 + 3].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) - 337 * 2 - 3].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) + 337 * 3 - 3].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) + 337 * 2 + 3].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) + 337 * 3 + 2].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) + 337 * 3 + 3].p += this.VecNormal * (0.3f * profundidad);
                    switch (direccion)
                    {
                        case 1:
                            vertices[(int)(vertCentral) - 337 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 3 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 - 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 337 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 3].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 2:
                            vertices[(int)(vertCentral) - 337 * 3 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 3 + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 - 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 2].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 3:
                            vertices[(int)(vertCentral) - 337 * 3 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 + 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 + 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 + 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 3 + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 + 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 3].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 337 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 2].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 4:
                            vertices[(int)(vertCentral) - 337 - 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 - 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 - 1].p -= this.VecNormal * 0.4f * (profundidad);
                            vertices[(int)(vertCentral) - 337].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 5:
                            vertices[(int)(vertCentral) - 337 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 + 1].p -= this.VecNormal * 0.8f * (profundidad);
                            vertices[(int)(vertCentral) - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 337 - 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 337 + 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 - 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 6:
                            vertices[(int)(vertCentral) - 337 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 + 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 + 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2 + 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 337 * 3 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 7:
                            vertices[(int)(vertCentral) + 337 - 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 - 1].p -= this.VecNormal * (0.8f * profundidad);

                            vertices[(int)(vertCentral) - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 - 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 3 - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 3 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 3].p -= this.VecNormal * (0.6f * profundidad);

                            vertices[(int)(vertCentral) - 337 - 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 3].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 3 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 8:
                            vertices[(int)(vertCentral) + 337 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 3 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 3 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 - 3].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 2].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        default:
                            vertices[(int)(vertCentral) + 337 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2 + 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 + 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 337 * 3 + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 337 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 337 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 337 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                    }
                }
                this.malla1.UnlockVertexBuffer();
            }
            #endregion
            #region abollandoParedCorta
            else//se trata de una pared corta
            {
                float coordExactaY = calcularCoordenada(ptoColisionado.Y);
                float coordExactaX = calcularCoordenada(ptoColisionado.X);
                float posRelativaAlMuroX = -this.PosicionActual.X + coordExactaX;
                float posRelativaAlMuroY = -this.PosicionActual.Y + coordExactaY;
                short nivel = nivelDeImpacto(disparo);
                int profundidad = profDeImpacto(disparo);
                short direccion = dirDeImpacto(disparo);
                EstructVertice[] vertices = (EstructVertice[])this.malla1.LockVertexBuffer(typeof(EstructVertice), LockFlags.None, this.malla1.NumberVertices);
                float vertCentral = Math.Abs(posRelativaAlMuroY / 2.5f) * 177 + (posRelativaAlMuroX / 2.5f);//ubicando el vertice central

                if (nivel == 0) { vertices[(int)(vertCentral)].p -= this.VecNormal * (0.4f * profundidad); }
                if (nivel == 1)
                {
                    switch (direccion)
                    {
                        case 1:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 177)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 176)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 177)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 176)].p -= this.VecNormal * (0.35f * profundidad);
                            break;
                        case 2:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 177)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 176)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 177)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 176)].p -= this.VecNormal * (0.3f * profundidad);
                            break;
                        case 3:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 177)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 176)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 177)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 176)].p -= this.VecNormal * (0.3f * profundidad);
                            break;
                        case 4:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 177)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 176)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 177)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 176)].p -= this.VecNormal * (0.35f * profundidad);
                            break;
                        case 5:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 177)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 176)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 177)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 176)].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 6:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 177)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 176)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral + 177)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 176)].p -= this.VecNormal * (0.3f * profundidad);
                            break;
                        case 7:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 177)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral - 176)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 177)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 176)].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 8:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 177)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral - 176)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral + 177)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral + 176)].p -= this.VecNormal * (0.35f * profundidad);
                            break;
                        default:
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 177)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral - 176)].p -= this.VecNormal * (0.35f * profundidad);
                            vertices[(int)(vertCentral - 1)].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral + 1)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral + 177)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral + 176)].p -= this.VecNormal * (0.35f * profundidad);
                            break;
                    }

                }
                if (nivel == 2)
                {
                    vertices[(int)(vertCentral)].p -= this.VecNormal * (0.3f * profundidad);
                    for (int i = 1; i < 4; i++)
                    {
                        vertices[(int)(vertCentral) + i].p -= this.VecNormal * (0.3f * profundidad);
                        vertices[(int)(vertCentral) - i].p -= this.VecNormal * (0.3f * profundidad);
                    }
                    for (int n = 1; n < 4; n++)
                    {
                        vertices[(int)(vertCentral) + n * 177].p -= this.VecNormal * (0.3f * profundidad);
                        vertices[(int)(vertCentral) - n * 177].p -= this.VecNormal * (0.3f * profundidad);
                        for (int i = 1; i < 4; i++)
                        {
                            vertices[(int)(vertCentral) + n * 177 + i].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral) + n * 177 - i].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral) - n * 177 - i].p -= this.VecNormal * (0.3f * profundidad);
                            vertices[(int)(vertCentral) - n * 177 + i].p -= this.VecNormal * (0.3f * profundidad);
                        }
                    }
                    vertices[(int)(vertCentral) - 177 * 3 - 3].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) - 177 * 3 - 2].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) - 177 * 3 + 3].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) - 177 * 2 - 3].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) + 177 * 3 - 3].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) + 177 * 2 + 3].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) + 177 * 3 + 2].p += this.VecNormal * (0.3f * profundidad);
                    vertices[(int)(vertCentral) + 177 * 3 + 3].p += this.VecNormal * (0.3f * profundidad);
                    switch (direccion)
                    {
                        case 1:
                            vertices[(int)(vertCentral) - 177 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 3 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 - 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 177 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 3].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177].p -= this.VecNormal * (0.4f * profundidad);

                            break;
                        case 2:
                            vertices[(int)(vertCentral) - 177 * 3 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 3 + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 - 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 2].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 3:
                            vertices[(int)(vertCentral) - 177 * 3 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 + 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 + 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 + 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 3 + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 + 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 3].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 177 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 2].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 4:
                            vertices[(int)(vertCentral) - 177 - 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 - 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 - 1].p -= this.VecNormal * 0.4f * (profundidad);
                            vertices[(int)(vertCentral) - 177].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 5:
                            vertices[(int)(vertCentral) - 177 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 + 1].p -= this.VecNormal * 0.8f * (profundidad);
                            vertices[(int)(vertCentral) - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 177 - 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 177 + 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 - 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 6:
                            vertices[(int)(vertCentral) - 177 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 + 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 + 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2 + 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 177 * 3 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 7:
                            vertices[(int)(vertCentral) + 177 - 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 - 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 - 1].p -= this.VecNormal * (0.8f * profundidad);

                            vertices[(int)(vertCentral) - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 - 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 3 - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 3 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 3].p -= this.VecNormal * (0.6f * profundidad);

                            vertices[(int)(vertCentral) - 177 - 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 3].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 3 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        case 8:
                            vertices[(int)(vertCentral) + 177 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 - 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 3].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 3 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 - 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 3 - 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 2].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 - 3].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 2].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                        default:
                            vertices[(int)(vertCentral) + 177 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 + 1].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2 + 2].p -= this.VecNormal * (0.8f * profundidad);
                            vertices[(int)(vertCentral)].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 + 3].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 2].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) + 177 * 3 + 1].p -= this.VecNormal * (0.6f * profundidad);
                            vertices[(int)(vertCentral) - 177 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) - 177 + 1].p -= this.VecNormal * (0.4f * profundidad);
                            vertices[(int)(vertCentral) + 177 - 1].p -= this.VecNormal * (0.4f * profundidad);
                            break;
                    }
                }
                this.malla1.UnlockVertexBuffer();
            #endregion
            }
        }
        public short nivelDeImpacto(Disparo disparo)
        {
            float fuerrido = disparo.fuerzaDeLanzamiento * disparo.recorridoLanzamiento;//es un factor para determinar el abollamiento que es la combinacion de fuerza y recorrido [0-300.000]
            if (disparo.colisionador.radio < 4)
            {
                if (fuerrido < 150000) { return 0; }
                else { return 1; }
            }
            if (disparo.colisionador.radio < 7)
            {
                if (fuerrido < 150000) { return 1; }
                else { return 2; }
            }
            if (disparo.colisionador.radio < 10)
            {
                if (fuerrido < 100000) { return 1; }
                else { return 2; }
            }
            if (disparo.colisionador.radio > 9)
            {
                if (fuerrido < 40000) { return 1; }
            }
            return 2;
        }
        public int profDeImpacto(Disparo disparo)
        {
            float factorHundimiento = disparo.recorridoLanzamiento * disparo.fuerzaDeLanzamiento * disparo.colisionador.Masa / 10;
            if (factorHundimiento < 70000) { return 1; }
            if (factorHundimiento >= 70000 && factorHundimiento < 150000) { return 2; }
            if (factorHundimiento >= 150000 && factorHundimiento < 230000) { return 3; }
            return 4;
        }
        public short dirDeImpacto(Disparo disparo)
        {//si el impacto deforma la pared en forma de circulo este puede contenerse en un cuadrado dividido en 9 cuadrantes, entonces aca se determina cual
            //de ellos se hunde con mayor profundidad segun la direccion del movimiento determinado por el vector velocidad al momento de la colision, para calcular en que angulo
            //impacto sin hacer operaciones de sin o cos, tomamos la razon entre las componentes del vector velocidad, una "tangente", y si el angulo es de 30º o menos
            //con respecto a la horizontal es el centro el que se deforma con mas profundidad, sino se elige uno de lo cuadrantes.
            float componenteXDeVel = disparo.velociadActual.X - disparo.colisionador.PosicionActual.X;
            float componenteYDeVel = disparo.velociadActual.Y - disparo.colisionador.PosicionActual.Y;
            float componenteZDeVel = disparo.velociadActual.Z - disparo.colisionador.PosicionActual.Z;
            if (this.VecNormal.X != 0) //pared paralela al plano yz
            {
                float tgX = componenteYDeVel / componenteXDeVel;
                float tgZ = componenteXDeVel / componenteZDeVel;
                if ((tgX > 0 && tgX <= 0.6f) && (tgZ > 0 && tgZ <= 0.6)) { return 5; }//el cuadrante del medio
                if ((tgX < 0 && tgX >= -0.6f) && (tgZ < 0 && tgZ >= -06f)) { return 5; }
                if (tgX > 0.6f && tgZ < -0.6) { return 1; }
                if (tgX > 0.6f && (tgZ <= 0.6f && tgZ >= -0.6f)) { return 2; }
                if (tgX > 0.6f && tgZ > 0.6f) { return 3; }
                if ((tgX >= -0.6f && tgX <= 0.6f) && tgZ < -0.6f) { return 4; }
                if ((tgX >= -0.6f && tgX <= 0.6f) && tgZ > 0.6f) { return 6; }
                if (tgX < -0.6f && tgZ < -0.6f) { return 7; }
                if (tgX < -0.6f && (tgZ >= -0.6f && tgZ < 0.6f)) { return 8; }
                if (tgX < -0.6f && tgZ > 0.6f) { return 9; }
            }
            if (this.VecNormal.Z != 0)//pared paralela al plano xy
            {
                float tgY = componenteYDeVel / componenteZDeVel;
                float tgX = componenteZDeVel / componenteXDeVel;
                if ((tgX > 0 && tgX <= 0.6f) && (tgY > 0 && tgY <= 0.6)) { return 5; }//el cuadrante del medio
                if ((tgX < 0 && tgX >= -0.6f) && (tgY < 0 && tgY >= -06f)) { return 5; }
                if (tgX > 0.6f && tgY < -0.6) { return 1; }
                if (tgX > 0.6f && (tgY <= 0.6f && tgY >= -0.6f)) { return 2; }
                if (tgX > 0.6f && tgY > 0.6f) { return 3; }
                if ((tgX >= -0.6f && tgX <= 0.6f) && tgY < -0.6f) { return 4; }
                if ((tgX >= -0.6f && tgX <= 0.6f) && tgY > 0.6f) { return 6; }
                if (tgX < -0.6f && tgY < -0.6f) { return 7; }
                if (tgX < -0.6f && (tgY >= -0.6f && tgY < 0.6f)) { return 8; }
                if (tgX < -0.6f && tgY > 0.6f) { return 9; }

            }
            return 10;
        }
        //public Vector3 deformarPared(Pared pared, Disparo disparo)
        //{
        //    Vector3[] dirVertices2 = null;
        //    Device dispositivo = GuiController.Instance.D3dDevice;
        //    VertexBuffer vertexBufferTemp;
        //    Vector3 verticeMasCercano, normalInvertida, ptoColisionado;
        //    meshPared = pared.malla1;
        //    float dist;

        //    normalInvertida.X = pared.VecNormal.X * -1;
        //    normalInvertida.Y = pared.VecNormal.Y * -1;
        //    normalInvertida.Z = pared.VecNormal.Z * -1;
        //    ptoColisionado.X = normalInvertida.X * (disparo.colisionador.esferaBounding.radio) + disparo.colisionador.PosicionActual.X;
        //    ptoColisionado.Y = normalInvertida.Y * (disparo.colisionador.esferaBounding.radio) + disparo.colisionador.PosicionActual.Y;
        //    ptoColisionado.Z = normalInvertida.Z * (disparo.colisionador.esferaBounding.radio) + disparo.colisionador.PosicionActual.Z;

        //    vertexBufferTemp = new VertexBuffer(typeof(EstructVertice), meshPared.NumberVertices, dispositivo, Usage.None, EstructVertice.Formato, Pool.Default);
        //    //lockeando el buffer
        //    EstructVertice[] vertices = (EstructVertice[])meshPared.LockVertexBuffer(typeof(EstructVertice), LockFlags.None, meshPared.NumberVertices);
        //    //cargo las direcciones de los vertices en el array dirVertices
        //    dirVertices2 = new Vector3[vertices.Length];
        //    for (int i = 0; i < vertices.Length; i++)
        //    {
        //        dirVertices2[i] = vertices[i].p + this.PosicionActual;
        //    }
        //    //buscando cual de los vertices de dirVertices es el mas cercano a la posicion de la colision, la distancia se devuelve en dist
        //    verticeMasCercano = TgcCollisionUtils.closestPoint(ptoColisionado, dirVertices2, out dist);
        //    if (disparo.colisionador.esferaBounding.radio < 7)
        //    {
        //        for (int i = 0; i < dirVertices2.Length; i++)
        //        {
        //            if (vertices[i].p + this.PosicionActual == verticeMasCercano)
        //            {//modifico el vertice central
        //                vertices[i].p -= this.vecNormal * 0.5f;
        //                //modifico los que estan alrededor

        //            }
        //        }
        //    }
        //    //hundiendo el vertice de la pared que quedó más cerca del colisionador
        //    for (int i = 0; i < dirVertices2.Length; i++)
        //    {
        //        if (vertices[i].p + this.PosicionActual == verticeMasCercano)
        //        {
        //            vertices[i].p -= this.VecNormal * (0.3f);//normalInvertida;

        //        }
        //    }

        //    //deslockeando el buffer
        //    meshPared1.UnlockVertexBuffer();
        //    //verticeMasCercano = this.puntoMasCercano(disparo.colisionador.PosicionActual, dirVertices2);
        //    return verticeMasCercano;//

        //}
        public void crearParedModuloOrientada(String orientacion)
        {
            Device dispositivo = GuiController.Instance.D3dDevice;

            Mesh malla = new Mesh(40 * 2 * 64, 65 * 41, MeshFlags.Managed, CustomVertex.PositionNormalTextured.Format, dispositivo);
            CustomVertex.PositionNormalTextured[] VB = new CustomVertex.PositionNormalTextured[65 * 41];
            short[] indices = new short[6 * 40 * 64];
            int k = 0;
            for (int i = 0; i < 65; i++)
            {
                for (int j = 0; j < 41; j++)
                {
                    if (orientacion == "frente")
                    {
                        VB[k].Position = new Vector3((float)j * 2.5f, (float)i * -2.5f, 0);
                        VB[k].Normal = this.VecNormal;//new Vector3(0, 0, -1);
                    }
                    if (orientacion == "costado")
                    {
                        VB[k].Position = new Vector3(0, (float)i * -2.5f, (float)j * 2.5f);
                        VB[k].Normal = this.VecNormal;
                    }
                    VB[k].Tu = (float)j / 40;
                    VB[k].Tv = (float)i / 64;
                    k++;
                }
            }

            for (int i = 0; i < 64 * 40; i++)
            {
                int indice = i + i / 40;
                indices[6 * i] = (short)indice;
                indices[6 * i + 1] = (short)(indice + 1);
                indices[6 * i + 2] = (short)(indice + 40 + 1);
                indices[6 * i + 3] = (short)(indice + 40 + 1);
                indices[6 * i + 4] = (short)(indice + 1);
                indices[6 * i + 5] = (short)(indice + 40 + 2);
            }
            malla.SetVertexBufferData(VB, LockFlags.None);
            malla.SetIndexBufferData(indices, LockFlags.None);
            this.bufferDeIndices = indices;
            this.bufferDeVertices = VB;
            this.textura = TextureLoader.FromFile(dispositivo, GuiController.Instance.ExamplesMediaDir + this.RutaDeLaTextura);
            this.malla1 = malla;
        }
        public void crearParedLarga()
        {
            Device dispositivo = GuiController.Instance.D3dDevice;

            Mesh malla = new Mesh(336 * 2 * 80, 81 * 337, MeshFlags.Managed, CustomVertex.PositionNormalTextured.Format, dispositivo);
            CustomVertex.PositionNormalTextured[] VB = new CustomVertex.PositionNormalTextured[81 * 337];
            short[] indices = new short[6 * 336 * 80];
            int k = 0;
            for (int i = 0; i < 81; i++)
            {
                for (int j = 0; j < 337; j++)
                {
                    VB[k].Position = new Vector3(0, (float)i * -2.5f, (float)j * 2.5f);
                    VB[k].Normal = this.VecNormal;
                    VB[k].Tu = (float)j / 336;
                    VB[k].Tv = (float)i / 80;
                    k++;
                }
            }
            for (int i = 0; i < 80 * 336; i++)
            {
                int indice = i + i / 336;
                indices[6 * i] = (short)indice;
                indices[6 * i + 1] = (short)(indice + 1);
                indices[6 * i + 2] = (short)(indice + 336 + 1);
                indices[6 * i + 3] = (short)(indice + 336 + 1);
                indices[6 * i + 4] = (short)(indice + 1);
                indices[6 * i + 5] = (short)(indice + 336 + 2);
            }
            malla.SetVertexBufferData(VB, LockFlags.None);
            malla.SetIndexBufferData(indices, LockFlags.None);
            this.bufferDeIndices = indices;
            this.bufferDeVertices = VB;
            this.textura = TextureLoader.FromFile(dispositivo, GuiController.Instance.ExamplesMediaDir + this.RutaDeLaTextura);
            this.malla1 = malla;
        }
        //public void crearParedLargaIzq()
        //{
        //    Device dispositivo = GuiController.Instance.D3dDevice;
        //
        //    Mesh malla = new Mesh(336 * 2 * 80, 81 * 337, MeshFlags.Managed, CustomVertex.PositionNormalTextured.Format, dispositivo);
        //    CustomVertex.PositionNormalTextured[] VB = new CustomVertex.PositionNormalTextured[81 * 337];
        //    short[] indices = new short[6 * 336 * 80];
        //    int k = 0;
        //    for (int i = 0; i < 81; i++)
        //    {
        //        for (int j = 0; j < 337; j++)
        //        {
        //            VB[k].Position = new Vector3(0, (float)i * -2.5f, (float)j * -2.5f);
        //            VB[k].Normal = this.VecNormal;
        //            VB[k].Tu = (float)j / 336;
        //            VB[k].Tv = (float)i / 80;
        //            k++;
        //        }
        //    }
        //    for (int n=0;n<80;n++)
        //        {
        //        for (int i = 0; i < 336; i++)
        //            {
        //            indices[n*336*6+6*i] = (short)(336+n*337 -i);
        //            indices[n*336*6+6*i+1] = (short)(336+n*337-i - 1);
        //            indices[n*336*6+6*i+ 2] = (short)(336+n*337+337-i);
        //            indices[n*336*6+6*i+ 3] = (short)(336+n*337+337-i);
        //            indices[n*336*6+6*i+ 4] = (short)(336+n*337-i - 1);
        //            indices[n*336*6+6*i+ 5] = (short)(336+n*337+337-1-i);
        //            }
        //        }
        //    malla.SetVertexBufferData(VB, LockFlags.None);
        //    malla.SetIndexBufferData(indices, LockFlags.None);
        //    this.bufferDeIndices = indices;
        //    this.bufferDeVertices = VB;
        //    this.textura = TextureLoader.FromFile(dispositivo, GuiController.Instance.ExamplesMediaDir + this.RutaDeLaTextura);
        //    this.malla1 = malla;
        //}
        public void crearParedCorta()
        {
            Device dispositivo = GuiController.Instance.D3dDevice;

            Mesh malla = new Mesh(176 * 2 * 80, 81 * 177, MeshFlags.Managed, CustomVertex.PositionNormalTextured.Format, dispositivo);
            CustomVertex.PositionNormalTextured[] VB = new CustomVertex.PositionNormalTextured[81 * 177];
            short[] indices = new short[6 * 176 * 80];
            int k = 0;
            for (int i = 0; i < 81; i++)
            {
                for (int j = 0; j < 177; j++)
                {
                    VB[k].Position = new Vector3((float)j * 2.5f, (float)i * -2.5f, 0);
                    VB[k].Normal = this.VecNormal;//new Vector3(0, 0, -1);
                    VB[k].Tu = (float)j / 176;
                    VB[k].Tv = (float)i / 80;
                    k++;
                }
            }

            for (int i = 0; i < 80 * 176; i++)
            {
                int indice = i + i / 176;
                indices[6 * i] = (short)indice;
                indices[6 * i + 1] = (short)(indice + 1);
                indices[6 * i + 2] = (short)(indice + 176 + 1);
                indices[6 * i + 3] = (short)(indice + 176 + 1);
                indices[6 * i + 4] = (short)(indice + 1);
                indices[6 * i + 5] = (short)(indice + 176 + 2);
            }
            malla.SetVertexBufferData(VB, LockFlags.None);
            malla.SetIndexBufferData(indices, LockFlags.None);
            this.bufferDeIndices = indices;
            this.bufferDeVertices = VB;
            this.textura = TextureLoader.FromFile(dispositivo, GuiController.Instance.ExamplesMediaDir + this.RutaDeLaTextura);
            this.malla1 = malla;
        }
        public void renderizar()
        {
            if (this.funcion == "piso" || this.funcion == "techo")
            {
                this.renderizarGral();
            }
            if (this.funcion == "paredModulo")
            {
                Device dispositivo = GuiController.Instance.D3dDevice;
                dispositivo.Transform.World = Matrix.Translation(PosicionActual);
                dispositivo.SetTexture(0, this.textura);
                this.malla1.DrawSubset(0);
            }
            TgcFrustum frustum = GuiController.Instance.Frustum;
            if ((this.funcion == "paredLarga" || this.funcion == "paredCorta"))
            {
                 Device dispositivo = GuiController.Instance.D3dDevice;
                dispositivo.Transform.World = Matrix.Translation(PosicionActual);
                efecto2.SetValue("matWorldViewProj", dispositivo.Transform.World * dispositivo.Transform.View * dispositivo.Transform.Projection);
                efecto2.SetValue("matWorld", dispositivo.Transform.World);
                efecto2.SetValue("matWorldView", dispositivo.Transform.World * dispositivo.Transform.View);
                if (this.VecNormal.X == -1) { efecto2.SetValue("tipoPared", 1); efecto2.SetValue("vecDirLuz", new Vector4(0.5f, 2500, -1000, 0)); }
                if (this.VecNormal.X == 1) {efecto2.SetValue("tipoPared", 3); efecto2.SetValue("vecDirLuz", new Vector4(-0.5f, 2500, 1000, 0));}
                if (this.VecNormal.Z == -1) {efecto2.SetValue("tipoPared", 2); efecto2.SetValue("vecDirLuz", new Vector4(-300, 2500, -0.5f, 0));}
                if (this.VecNormal.Z == 1) {efecto2.SetValue("tipoPared", 4); efecto2.SetValue("vecDirLuz", new Vector4(-300, 2500, 0.5f, 0));}
                
                efecto2.SetValue("luzAmbiente", new Vector4(0.48f,0.50f,0.54f, 1));
                efecto2.SetValue("luzEspecular", new Vector4(0.65f, 0.60f, 0.60f, 1));
                efecto2.SetValue("luzDifusa", new Vector4(0.68f, 0.68f, 0.68f, 1));
                efecto2.SetValue("matDiffuse", 7.0f);
                efecto2.SetValue("matAmbiente", 1.65f);
                efecto2.SetValue("matEspecular", 01.40f);
                efecto2.SetValue("exponencial", 14.0f);
                if ((bool)GuiController.Instance.Modifiers["luzPared"]) { efecto2.Technique = "BlinnPhong"; }
                else { efecto2.Technique = "Sombrear"; }
                int numPasadas = efecto2.Begin(0);
                for (int pas = 0; pas < numPasadas; pas++)
                {
                    efecto2.BeginPass(pas);
                    //for (int i = 0; i < meshMateriales.Length; i++)
                    //{
                    dispositivo.Transform.World = Matrix.Translation(PosicionActual);
                    dispositivo.SetTexture(0, this.textura);
                    this.malla1.DrawSubset(0);
                    //}
                }
                efecto2.EndPass();
                efecto2.End();
                //this.boundingBoxAsociado.render();
            }
        }
    }
}
