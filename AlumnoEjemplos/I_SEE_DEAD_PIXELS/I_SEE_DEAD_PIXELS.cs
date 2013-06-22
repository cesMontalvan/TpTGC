using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using TgcViewer;
using TgcViewer.Example;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.Collision;
using TgcViewer.Utils._2D;

using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace AlumnoEjemplos.I_SEE_DEAD_PIXELS
{

    public class I_SEE_DEAD_PIXELS : TgcExample
    {
        /// <summary>
        /// Categoría a la que pertenece el ejemplo.
        /// Influye en donde se va a haber en el árbol de la derecha de la pantalla.
        /// </summary>
        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }
        /// <summary>
        /// Completar nombre del grupo en formato Grupo NN
        /// </summary>
        public override string getName()
        {
            return "Grupo I_SEE_DEAD_PIXELS ";
        }
        /// <summary>
        /// Completar con la descripción del TP
        /// </summary>
        public override string getDescription()
        {
            return "   TP de colisiones contra paredes deformables.   Movimiento:A,S,D,W.      Tomar esferas:R.   Tomar/Soltar arma: R  Arrojar/disparar: click derecho   Activar Camara Target: Presionar Q durante el clickeo";
        }

        Escenario escenario = new Escenario();
        float tiempoAcumulado = 0;
        Vector3 puntoMedioScreen = new Vector3(0, 0, 0);
        Vector3 puntoDePartida = new Vector3(0, 0, 0);
        public float velDeTiempo = 3.7f;
        public float VelDeTiempo
        {
            set { this.velDeTiempo = value; }
            get { return this.velDeTiempo; }
        }
        public TgcSprite spriteMira = new TgcSprite();
        public Size screenSize = GuiController.Instance.Panel3d.Size;
        public override void init()
        {
            Device dispositivo = GuiController.Instance.D3dDevice;
            dispositivo.RenderState.AntiAliasedLineEnable = true;
            this.spriteMira.Texture = TgcTexture.createTexture(GuiController.Instance.ExamplesMediaDir + "ModelosPropios\\arma3.png");
            Size textureSize = spriteMira.Texture.Size;
            spriteMira.Position = new Vector2(FastMath.Max(screenSize.Width / 2 - textureSize.Width / 2, 0), FastMath.Max(screenSize.Height / 2 - textureSize.Height / 2, 0));
            GuiController.Instance.FpsCamera.Enable = true;
            //GuiController.Instance.FpsCamera.setCamera(new Vector3(0, 10, 0), new Vector3(0, 0, 0));
            GuiController.Instance.Modifiers.addFloat("fuerza", 1000f, 99999f, 30000f);
            GuiController.Instance.Modifiers.addFloat("masa", 1f, 100f, 40);
            GuiController.Instance.Modifiers.addFloat("recorrido del impulso", 0.1f, 3, 1.3f);
            GuiController.Instance.Modifiers.addBoolean("vectoVel", "Mostrar Vector Velocidad", false);
            GuiController.Instance.Modifiers.addBoolean("luzAmb","Luz Ambiente", false);
            GuiController.Instance.Modifiers.addBoolean("luzDif", "Luz Difusa", false);
            GuiController.Instance.Modifiers.addBoolean("luzEspec", "Luz Especular (incluye ambiente y difusa esten o no seleccionadas)", false);
            GuiController.Instance.Modifiers.addBoolean("blinnPhong", "Shader de iluminacion Blinn-Phong(tiene prioridad sobre los demas)", false);
            GuiController.Instance.Modifiers.addBoolean("luzPared", "Iluminacion de Pared", false);
            escenario.inicializarEscenario();
       }

        public override void render(float elapsedTime)
        {
            Device dispositivo = GuiController.Instance.D3dDevice;
            if (escenario.camaraEspecialActivada == true)
            {
                //GuiController.Instance.ThirdPersonCamera.
                if (VelDeTiempo < 0.5f)
                {
                    //VelDeTiempo += 0.016f;
                }
                else
                {
                    if (escenario.armaActiva == null) { VelDeTiempo -= 0.019f; }
                    else { velDeTiempo -= 0.26f; }
                }
            }
            if (escenario.camaraEspecialActivada == false) { VelDeTiempo = 3.8f; }
            
            tiempoAcumulado += GuiController.Instance.ElapsedTime * velDeTiempo;
            escenario.actualizar(tiempoAcumulado);
            escenario.renderizar(tiempoAcumulado);

            //Iniciar dibujado de todos los Sprites de la escena (en este caso es solo uno)
            GuiController.Instance.Drawer2D.beginDrawSprite();
            this.spriteMira.render();
            //Finalizar el dibujado de Sprites
            GuiController.Instance.Drawer2D.endDrawSprite();
        }
        public override void close()
        {

        }

    }
}
