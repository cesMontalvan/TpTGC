using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using TgcViewer.Utils._2D;


namespace AlumnoEjemplos.I_SEE_DEAD_PIXELS
{
    public class Escenario
    {
        public bool camaraEspecialActivada = false;
        public TgcBox caja1,caja2,caja3,caja4,caja5;
        public Armas armaActiva = null;
        public List<Armas> armamento = new List<Armas>();
        public ManosArrojadoras arma1 = new ManosArrojadoras();
        public DeDisparo arma2 = new DeDisparo();
        public List<Disparo> disparosDeManos = new List<Disparo>();
        public List<Disparo> disparosDeDisparadores = new List<Disparo>();
        Vector3 direccionDeDisparo = new Vector3(0, 0, 0);
        public List<Pared> paredes = new List<Pared>();
        public Pared techo = new Pared();
        public Pared pisoX = new Pared();
        public struct EstructVertice //estructura usada para almacenar c/u de los elementos levantados del Vertex Buffer
        {
            public Vector3 p;
            public Vector3 n;
            public float tu, tv;
            public static readonly VertexFormats Formato = VertexFormats.Position | VertexFormats.Normal | VertexFormats.Texture1;
        }
        public Pared pared1 = new Pared();
        public Pared pared2 = new Pared();
        public Pared pared3 = new Pared();
        public Pared pared4 = new Pared();

        public void inicializarEscenario()
        {
            Device dispositivo1 = GuiController.Instance.D3dDevice;
            #region cargarHabitacion
            //inicializando techo
            techo.Tipo = "archivoX";
            techo.funcion = "techo";
            techo.PosicionInicial = new Vector3(0, 135, 400);
            techo.PosicionActual = new Vector3(0, 135, 400);
            techo.RutaDeLaTextura = "ModelosPropios\\Metal_texture_k_by_enframedBrillo.bmp";
            techo.RutaDelMesh = "ModelosPropios\\techo1200x536xx8.X";
              techo.inicializar(new Vector3(0, -1, 0));
            paredes.Add(techo);
            //inicializando piso de un archivo X
            pisoX.Tipo = "archivoX";
            pisoX.funcion = "piso";
            pisoX.PosicionInicial = new Vector3(0, -25, 300);
            pisoX.PosicionActual = new Vector3(0, -25, 300);
            pisoX.RutaDelMesh = "ModelosPropios\\pisoPlano830x430.X";
            pisoX.RutaDeLaTextura = "ModelosPropios\\Textura Metal Riscado.bmp";
            pisoX.inicializar(new Vector3(0, 1, 0));
            paredes.Add(pisoX);
            //inicializando las mesh paredes
            pared1.Tipo = "archivoX";
            pared1.funcion = "paredLarga";
            pared1.PosicionInicial = new Vector3(200.00f, 155, -120);
            pared1.PosicionActual = new Vector3(200.00f, 155, -120);
            pared1.RutaDeLaTextura = "ModelosPropios\\Metal_texture_k_by_enframedOsucra.bmp";
            pared1.inicializar(new Vector3(-1, 0, 0));
            paredes.Add(pared1);
            pared2.Tipo = "archivoX";
            pared2.funcion = "paredCorta";
            pared2.PosicionInicial = new Vector3(-220, 155, 700.00f);
            pared2.PosicionActual = new Vector3(-220, 155, 700.00f);
            pared2.RutaDeLaTextura = "ModelosPropios\\Metal_texture_k_by_enframed.bmp";
            pared2.inicializar(new Vector3(0, 0, -1));
            paredes.Add(pared2);
            pared3.Tipo = "archivoX";
            pared3.funcion = "paredLarga";
            pared3.PosicionInicial = new Vector3(-200.00f, 155, -120);
            pared3.PosicionActual = new Vector3(-200.00f, 155, -120);
            pared3.RutaDeLaTextura = "ModelosPropios\\Metal_texture_k_by_enframed.bmp";
            pared3.inicializar(new Vector3(1, 0, 0));
            paredes.Add(pared3);
            pared4.Tipo = "archivoX";
            pared4.funcion = "paredCorta";
            pared4.PosicionInicial = new Vector3(-220, 155, -100.00f);
            pared4.PosicionActual = new Vector3(-220, 155, -100.00f);
            pared4.RutaDeLaTextura = "ModelosPropios\\Metal_texture_k_by_enframed.bmp";
            pared4.inicializar(new Vector3(0, 0, 1));
            paredes.Add(pared4);
            arma2.modelo = new Malla();
            arma2.modelo.Tipo = "archivoX";
            arma2.modelo.PosicionActual = new Vector3(0, -20.0f, 45);
            arma2.modelo.PosicionInicial = new Vector3(0, -20.0f, 45);
            arma2.modelo.RutaDelMesh = "ModelosPropios\\arma2Tirada.X";
            arma2.modelo.RutaDeLaTextura = "ModelosPropios\\CS-Black-Texture.bmp";
            arma2.modelo.inicializarGral();
            #endregion
            arma1.inicializar();
            armamento.Add(arma1);
            armamento.Add(arma2);
            foreach (Colisionador arrojable in arma1.arrojables)
            {
                Disparo disparo = new Disparo(arrojable, new Vector3(0.01f, 0.01f, 0.01f), 1);
                disparo.Alfa = 0;
                this.disparosDeManos.Add(disparo);
            }
            caja1 = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(5, 5, 5),Color.Black);
            caja2 = TgcBox.fromSize(new Vector3(10, 0, 0), new Vector3(5, 5, 5),Color.Olive);
            caja3 = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(5, 5, 5),Color.Brown);
            caja4 = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(5, 5, 5),Color.Coral);
        }
        public void actualizar(float instante)
        {
            Device dispositivo = GuiController.Instance.D3dDevice;
            if (GuiController.Instance.D3dInput.buttonPressed(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_RIGHT) && armaActiva != null&&camaraEspecialActivada==false)
            {
                Vector3 puntoDePartida = Vector3.TransformCoordinate(new Vector3(0, -0.5f, 0.5f), Matrix.Invert(dispositivo.Transform.View));
                Vector3 puntoMedioScreen = Vector3.TransformCoordinate(new Vector3(0, 0, 10), Matrix.Invert(dispositivo.Transform.View));
                if ((armaActiva.GetType() == typeof(ManosArrojadoras)) && arma1.arrojableCargado != null)
                {
                    //posicion inicial del colisionador
                    arma1.arrojableCargado.PosicionInicial = GuiController.Instance.FpsCamera.Position;
                    arma1.arrojableCargado.PosicionActual = GuiController.Instance.FpsCamera.Position;
                    //direccionando el disparo
                    arma1.arrojableCargado.Masa = (float)GuiController.Instance.Modifiers["masa"];
                    Vector3 nuevaDireccion = puntoMedioScreen - puntoDePartida;
                    foreach (Disparo disparo in disparosDeManos)
                    {
                        if (disparo.colisionador == arma1.arrojableCargado)
                        {
                            //disparo.colisionador.estadoDinamico = 1;
                            disparo.recorridoLanzamiento = (float)GuiController.Instance.Modifiers["recorrido del impulso"];
                            disparo.fuerzaDeLanzamiento = (float)GuiController.Instance.Modifiers["fuerza"];
                            disparo.InstanteDeDisparo = instante;
                            disparo.DirDeDisparo = nuevaDireccion;
                            disparo.calcularAlfa();
                            disparo.setearVelocidadInicial();
                        }
                    }
                    if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.Q) && camaraEspecialActivada == false)
                    {
                        GuiController.Instance.FpsCamera.Enable = false;
                        GuiController.Instance.ThirdPersonCamera.Enable = true;
                        GuiController.Instance.ThirdPersonCamera.setCamera(arma1.arrojableCargado.PosicionActual, 19,-38);
                        //GuiController.Instance.ThirdPersonCamera.Target = arma1.arrojableCargado.PosicionActual;
                        //GuiController.Instance.ThirdPersonCamera.
                        this.camaraEspecialActivada = true;
                        arma1.arrojableCargado.conCamara = true;
                    }
                    arma1.arrojableCargado = null;
                    armaActiva = null;

                }
                else
                {
                    //disparar con el tipo de arma que sea
                    //crear el colisionador que se dispara con su masa posicion y tipo
                    Colisionador bala = new Colisionador(0.85f, GuiController.Instance.FpsCamera.Position, "archivoX", "ModelosPropios\\bala.X", "ModelosPropios\\de-oro.bmp");//no modificar bala.x -guarda que con una textura media pesada te caga la vida
                    //crear un disparo con ese colisionador en la direccion y el instante
                    ////Matrix.LookAtLH(GuiController.Instance.FpsCamera.Position,puntoMedioScreen,new Vector3(0,1,0));
                    disparosDeDisparadores.Add(new Disparo(bala, puntoMedioScreen - GuiController.Instance.FpsCamera.Position, instante));
                    bala.crearMatrizDeAlineacion(puntoMedioScreen - GuiController.Instance.FpsCamera.Position);
                    if (GuiController.Instance.D3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.Q)&&camaraEspecialActivada==false)
                        {
                        this.camaraEspecialActivada = true;
                        GuiController.Instance.FpsCamera.Enable = false;
                        GuiController.Instance.ThirdPersonCamera.Enable = true;
                        GuiController.Instance.ThirdPersonCamera.setCamera(bala.PosicionActual, 12,-14);//9,-18);
                        //GuiController.Instance.ThirdPersonCamera.
                        //GuiController.Instance.ThirdPersonCamera.
                        bala.conCamara = true;
                        }
                }
            }
            if (GuiController.Instance.D3dInput.keyPressed(Microsoft.DirectX.DirectInput.Key.R)&&camaraEspecialActivada==false)
            {
                Vector3 ptoMedioParaAgarrar = Vector3.TransformCoordinate(new Vector3(0, 0, 10), Matrix.Invert(dispositivo.Transform.View));
                foreach (Colisionador coli in arma1.arrojables)
                {
                    Vector3 posAbsInicio = GuiController.Instance.FpsCamera.Position - coli.PosicionActual;
                    Vector3 posAbsEnd = ptoMedioParaAgarrar - coli.PosicionActual;
                    if (coli.malla1.Intersect(posAbsInicio, posAbsEnd - posAbsInicio))
                    {
                        float dist = (GuiController.Instance.FpsCamera.Position.X - coli.PosicionActual.X) * (GuiController.Instance.FpsCamera.Position.X - coli.PosicionActual.X) * (GuiController.Instance.FpsCamera.Position.Y - coli.PosicionActual.Y);
                        if (dist < 40 * 40)
                        {
                            if (armaActiva == null)
                            {
                                armaActiva = arma1;
                                arma1.arrojableCargado = coli;
                            }
                            if (armaActiva != arma1)
                            {
                                armaActiva.volverAInicio();
                                armaActiva = arma1;
                                arma1.arrojableCargado = coli;
                            }
                            arma1.arrojableCargado = coli;
                        }
                    }
                }
                if (armaActiva == arma2)
                {
                    arma2.volverAInicio();
                    armaActiva = null;
                }
                if (armaActiva == null)
                {
                    Vector3 posAbsInicio = GuiController.Instance.FpsCamera.Position - arma2.modelo.PosicionActual;
                    Vector3 posAbsEnd = ptoMedioParaAgarrar - arma2.modelo.PosicionActual;
                    if (arma2.modelo.malla1.Intersect(posAbsInicio, posAbsEnd - posAbsInicio))
                    {
                        float dist = (GuiController.Instance.FpsCamera.Position.X - arma2.modelo.PosicionActual.X) * (GuiController.Instance.FpsCamera.Position.X - arma2.modelo.PosicionActual.X) * (GuiController.Instance.FpsCamera.Position.Y - arma2.modelo.PosicionActual.Y);
                        if (dist < 40 * 40)
                        {
                            armaActiva = arma2;
                        }
                    }
                }
            }
        }
        public void renderizar(float instante)
        {
            #region recorridoDeDisparosEnElEscenario
            List<Disparo> listaAux = new List<Disparo>();
            listaAux.AddRange(disparosDeManos);
            listaAux.AddRange(disparosDeDisparadores);
            foreach (Disparo disparo in listaAux)
            {
                float radio = disparo.colisionador.radio;  

                if (disparosDeManos.Contains(disparo) && disparo.Alfa != 0)
                {
                    foreach (Colisionador colisio1 in arma1.arrojables)
                    {
                        if (colisio1 != disparo.colisionador)
                        {
                            Vector3 vecDistancia = disparo.colisionador.PosicionActual - colisio1.PosicionActual;
                            float normaVecDistancia=vecDistancia.X*vecDistancia.X+vecDistancia.Y*vecDistancia.Y+vecDistancia.Z*vecDistancia.Z;
                            float radiosAl2=(disparo.colisionador.radio+colisio1.radio)*(disparo.colisionador.radio+colisio1.radio);
                            if (normaVecDistancia <radiosAl2 )//ver como evitar length
                            {
                                Vector3 ptoInicio = disparo.colisionador.PosicionActual;
                                Vector3 nuevaDireccion = disparo.colisionador.PosicionActual - colisio1.PosicionActual;
                                //nueva posicion inicial del colisionador
                                disparo.colisionador.PosicionInicial = ptoInicio;
                                disparo.colisionador.PosicionActual = ptoInicio;
                                //redireccionamos el disparo
                                disparo.DirDeDisparo = nuevaDireccion * 0.3f;//dirVelReducida;
                                //le bajamos la velocidad con la que sale del rebote o choque
                                disparo.Alfa *= 0.5f; //introducir la masa para que influya en el frenado
                                //disparo.InstanteDeDisparo = instante;
                                //seteamos la nueva velocidad del disparo
                                disparo.setearVelocidadInicial();
                            }
                        }
                    }
                }
                if (disparo.colisionador.PosicionActual.X + radio > 200 && Vector3.Dot(disparo.velociadActual, new Vector3(-1, 0, 0)) < 0)
                    {
                        if (disparo.colisionador.RutaDelMesh == "ModelosPropios\\bala.X") { disparosDeDisparadores.Remove(disparo); }
                        if (camaraEspecialActivada == true) { this.desactivarCamara(disparo.colisionador.PosicionInicial,disparo.colisionador.PosicionActual); disparo.colisionador.conCamara = false; }
                        chocar(disparo, this.pared1.VecNormal, instante);
                        this.pared1.deformarse(disparo);
                    }
                    else
                    {
                        if (disparo.colisionador.PosicionActual.X - radio < -200 && Vector3.Dot(disparo.velociadActual, new Vector3(1, 0, 0)) < 0)
                        {
                            if (disparo.colisionador.RutaDelMesh == "ModelosPropios\\bala.X") { disparosDeDisparadores.Remove(disparo); }
                            if (camaraEspecialActivada == true) { this.desactivarCamara(disparo.colisionador.PosicionInicial, disparo.colisionador.PosicionActual); disparo.colisionador.conCamara = false; }
                            chocar(disparo, this.pared3.VecNormal, instante);
                            this.pared3.deformarse(disparo);
                        }
                        else
                        {
                            if (disparo.colisionador.PosicionActual.Z + radio > 700 && Vector3.Dot(disparo.velociadActual, new Vector3(0, 0, -1)) < 0)
                            {
                                if (disparo.colisionador.RutaDelMesh == "ModelosPropios\\bala.X") { disparosDeDisparadores.Remove(disparo); }
                                if (camaraEspecialActivada == true) { this.desactivarCamara(disparo.colisionador.PosicionInicial, disparo.colisionador.PosicionActual); disparo.colisionador.conCamara = false; }
                                chocar(disparo, this.pared2.VecNormal, instante);
                                this.pared2.deformarse(disparo);
                            }
                            else
                            {
                                if (disparo.colisionador.PosicionActual.Z - radio < -100 && Vector3.Dot(disparo.velociadActual, new Vector3(0, 0, 1)) < 0)
                                {
                                    if (disparo.colisionador.RutaDelMesh == "ModelosPropios\\bala.X") { disparosDeDisparadores.Remove(disparo); }
                                    if (camaraEspecialActivada == true) { this.desactivarCamara(disparo.colisionador.PosicionInicial, disparo.colisionador.PosicionActual); disparo.colisionador.conCamara = false; }
                                    chocar(disparo, this.pared4.VecNormal, instante);
                                    this.pared4.deformarse(disparo);
                                }
                                else
                                {
                                    if ((disparo.colisionador.PosicionActual.Y + radio > 135) && Vector3.Dot(disparo.velociadActual, new Vector3(0, -1, 0)) < 0)
                                    {
                                        if (disparo.colisionador.RutaDelMesh == "ModelosPropios\\bala.X") { disparosDeDisparadores.Remove(disparo); }
                                        if (camaraEspecialActivada == true) { this.desactivarCamara(disparo.colisionador.PosicionInicial, disparo.colisionador.PosicionActual); disparo.colisionador.conCamara = false; }
                                        chocar(disparo, this.techo.VecNormal, instante);
                                    }
                                    else
                                    {
                                        if ((disparo.colisionador.PosicionActual.Y - radio < -25) && (Vector3.Dot(disparo.velociadActual, new Vector3(0, 1, 0)) < 0))
                                        {
                                            if (disparo.colisionador.RutaDelMesh == "ModelosPropios\\bala.X") { disparosDeDisparadores.Remove(disparo); }
                                            if (camaraEspecialActivada == true) { this.desactivarCamara(disparo.colisionador.PosicionInicial, disparo.colisionador.PosicionActual); disparo.colisionador.conCamara = false; }
                                            chocar(disparo, this.pisoX.VecNormal, instante);
                                        }
                                    }
                                }
                            }
                        }
                    }
                if (disparo.colisionador.conCamara==true)
                {
                    //GuiController.Instance.ThirdPersonCamera. -= 0.01f;// osition = disparo.colisionador.PosicionActual - new Vector3(1, 1, 1);
                    GuiController.Instance.ThirdPersonCamera.Target = disparo.colisionador.PosicionActual; 
                }
                if (disparo.colisionador != arma1.arrojableCargado) { disparo.renderizar(instante); }
            }
            listaAux.Clear();
            #endregion
            //this.determinarRenderParedes();
            foreach (Pared pared in paredes){ pared.renderizar(); }
            if (this.armaActiva != arma2)
            {
                arma2.modelo.renderizarGral();
            }
           
        }
        public void chocar(Disparo disparo1, Vector3 normalDeMuro, float tiempo)
        {
            Vector3 ptoInicio = disparo1.colisionador.PosicionActual;
            Vector3 ptoAux1 = new Vector3();
            Vector3 ptoAux11 = new Vector3();
            Vector3 ptoAux2 = new Vector3();
            Vector3 ptoAux21 = new Vector3();
            Vector3 ptoAux3 = new Vector3();
            Vector3 ptoAux31 = new Vector3();
            Vector3 dirVelReducida = new Vector3();

            float proyeccion;
            ptoAux1.X = disparo1.velociadActual.X * (-1);
            ptoAux1.Y = disparo1.velociadActual.Y * (-1);
            ptoAux1.Z = disparo1.velociadActual.Z * (-1);
            ptoAux11 = ptoAux1 + ptoInicio;
            proyeccion = Vector3.Dot(ptoAux1, normalDeMuro);
            ptoAux2.X = proyeccion * normalDeMuro.X;
            ptoAux2.Y = proyeccion * normalDeMuro.Y;
            ptoAux2.Z = proyeccion * normalDeMuro.Z;
            ptoAux21 = ptoAux2 + ptoInicio;
            ptoAux3 = ptoAux21 - ptoAux11;
            ptoAux31 = ptoAux21 + ptoAux3;
            dirVelReducida = ptoAux31 - ptoInicio;
            dirVelReducida.Normalize();
            dirVelReducida.Scale(disparo1.dirDeDisparo.Length());
            Vector3 ptoFin = ptoAux31;

            //nueva posicion inicial del colisionador
            disparo1.colisionador.PosicionInicial = ptoInicio;
            disparo1.colisionador.PosicionActual = ptoInicio;
            //redireccionamos el disparo
            //Vector3 nuevaDireccion = ptoFin - ptoInicio;
            disparo1.DirDeDisparo = dirVelReducida;
            //le bajamos la velocidad con la que sale del rebote o choque
            if (disparo1.colisionador.RutaDelMesh == "ModelosPropios\\bala.X")
            {
                disparo1.velociadActual.X = 0;
                disparo1.velociadActual.Z = 0;
            }
            else
            { disparo1.Alfa *= 0.8f; }//*0.05f*disparo1.colisionador.Masa; }//introducir la masa para que influya en el frenado
            disparo1.InstanteDeDisparo = tiempo;
            //seteamos la nueva velocidad del disparo
            disparo1.setearVelocidadInicial();
        }
        public void desactivarCamara(Vector3 posCamara, Vector3 dirMirada)
        {
            GuiController.Instance.ThirdPersonCamera.Enable = false;
            GuiController.Instance.FpsCamera.Enable = true;
            GuiController.Instance.FpsCamera.setCamera(posCamara,dirMirada);
            this.camaraEspecialActivada = false;
            
        }
        public void determinarRenderParedes()
        {
            Device dispositivo = GuiController.Instance.D3dDevice;
            
            //calculo las paredes que no son visibles creando un cono de vision de 90º-el frustum- y chequeo que paredes son renderizadas
            //Vector3 vecLookAt=GuiController.Instance.CurrentCamera.getLookAt();
            
            
            Vector3 vecMundoObj=GuiController.Instance.CurrentCamera.getPosition();
            Vector3 vecLookAt = Vector3.TransformCoordinate(new Vector3(0, 0, 10), Matrix.Invert(dispositivo.Transform.View))-GuiController.Instance.FpsCamera.Position;
            
            Vector3 vecObjW = vecLookAt - vecMundoObj;//vecMundoObj + vecLookAt;
            Vector3 vecRotadoIzq= Vector3.TransformCoordinate(vecObjW, Matrix.RotationY((float)Math.PI/ 4));
            Vector3 vecRotadoDer = Vector3.TransformCoordinate(vecObjW, Matrix.RotationY((float)-Math.PI / 4));
            int pared1Tocada=0;
            int pared2Tocada=0;
            int pared3Tocada=0;
            int pared4Tocada=0;

            if (Vector3.Dot(pared1.VecNormal, vecLookAt) == 0) { pared1.renderizar(); pared2.renderizar(); pared3.renderizar(); pisoX.renderizar(); techo.renderizar(); }
            if (Vector3.Dot(pared2.VecNormal, vecLookAt) == 0) { pared1.renderizar(); pared3.renderizar(); pared4.renderizar(); pisoX.renderizar(); techo.renderizar(); }
            if (Vector3.Dot(pared4.VecNormal, vecLookAt) == 0) { pared1.renderizar(); pared2.renderizar(); pared3.renderizar(); pisoX.renderizar(); techo.renderizar(); }

            float pendRectaIzq = (vecRotadoDer.Z / vecRotadoDer.X);
            if((200-vecMundoObj.X)*pendRectaIzq<=(705-vecMundoObj.Z)&&(200-vecMundoObj.X)*pendRectaIzq>=(-105-vecMundoObj.Z)){pared1Tocada=+1;}
            if((-200-vecMundoObj.X)*pendRectaIzq<=(705-vecMundoObj.Z)&&(-200-vecMundoObj.X)*pendRectaIzq>=(-105-vecMundoObj.Z)){pared2Tocada=+1;}
            if((700-vecMundoObj.Z)/pendRectaIzq<=(205-vecMundoObj.X)&&(700-vecMundoObj.Z)/pendRectaIzq>=(-205-vecMundoObj.X)){pared3Tocada=+1;}
            if((-100-vecMundoObj.Z)/pendRectaIzq<=(205-vecMundoObj.X)&&(-100-vecMundoObj.Z)/pendRectaIzq>=(-205-vecMundoObj.X)){pared4Tocada=+1;}

            float pendRectaDer = (vecRotadoIzq.Z/ vecRotadoIzq.X);
            if((200-vecMundoObj.X)*pendRectaDer<=(705-vecMundoObj.Z)&&(200-vecMundoObj.X)*pendRectaDer>=(-105-vecMundoObj.Z)){pared1Tocada=+1;}
            if((-200-vecMundoObj.X)*pendRectaDer<=(705-vecMundoObj.Z)&&(-200-vecMundoObj.X)*pendRectaDer>=(-105-vecMundoObj.Z)){pared2Tocada=+1;}
            if((700-vecMundoObj.Z)/pendRectaDer<=(205-vecMundoObj.X)&&(700-vecMundoObj.Z)/pendRectaDer>=(-205-vecMundoObj.X)){pared3Tocada=+1;}
            if((-100-vecMundoObj.Z)/pendRectaDer<=(205-vecMundoObj.X)&&(-100-vecMundoObj.Z)/pendRectaDer>=(-205-vecMundoObj.X)){pared4Tocada=+1;}

            if (pared1Tocada == 2 && vecLookAt.X > 0) { pared1.renderizar(); pisoX.renderizar(); techo.renderizar(); caja1.move(new Vector3(-200 - vecMundoObj.X, 40, 699)); }
            //if(pared2Tocada==2&&vecObjW.Z>0){pared2.renderizar();pisoX.renderizar();techo.renderizar();}
            //if(pared3Tocada==2&&vecLookAt.X<0){pared3.renderizar();pisoX.renderizar();techo.renderizar();}
            //if(pared4Tocada==2&&vecLookAt.Z<0){pared4.renderizar();pisoX.renderizar();techo.renderizar();}

            if(pared1Tocada==1&&pared2Tocada==1&&vecLookAt.X>0&&vecLookAt.Z>0){pared1.renderizar();pared2.renderizar();pisoX.renderizar();techo.renderizar();}
            //if(pared2Tocada==1&&pared3Tocada==1&&vecLookAt.X<0&&vecLookAt.Z>0){pared2.renderizar();pared3.renderizar();pisoX.renderizar();techo.renderizar();}
            //if(pared3Tocada==1&&pared4Tocada==1&&vecLookAt.X<0&&vecLookAt.Z<0){pared3.renderizar();pared4.renderizar();pisoX.renderizar();techo.renderizar();}
            //if(pared4Tocada==1&&pared1Tocada==1&&vecLookAt.X<0&&vecLookAt.Z>0){pared1.renderizar();pared4.renderizar();pisoX.renderizar();techo.renderizar();}

            //if(pared1Tocada==1&&pared3Tocada==1&&vecObjW.Z>0){pared1.renderizar();pared2.renderizar();pared3.renderizar();pisoX.renderizar();techo.renderizar();}
            //if(pared2Tocada==1&&pared4Tocada==1&&vecLookAt.X<0){pared2.renderizar();pared3.renderizar();pared4.renderizar();techo.renderizar();pisoX.renderizar();}
            //if(pared3Tocada==1&&pared1Tocada==1&&vecLookAt.Z<0){pared1.renderizar();pared4.renderizar();pared3.renderizar();pisoX.renderizar();techo.renderizar();}
            //if(pared2Tocada==1&&pared4Tocada==1&&vecLookAt.X>0){pared2.renderizar();pared1.renderizar();pared2.renderizar();pisoX.renderizar();techo.renderizar();}
            //caja1.render();
            //caja2.render();
            //caja3.render();
            //caja4.render();
        }

    }
}
