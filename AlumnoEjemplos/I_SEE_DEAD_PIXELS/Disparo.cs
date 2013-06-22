using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Modifiers;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace AlumnoEjemplos.I_SEE_DEAD_PIXELS
{
    public class Disparo
    {
        public float recorridoLanzamiento = (float)GuiController.Instance.Modifiers["recorrido del impulso"];//= 0.2f;//recorrido del empuje cuando se lanza el objeto
        public float fuerzaDeLanzamiento = (float)GuiController.Instance.Modifiers["fuerza"];//=40000;//constante por ahora, la tiene que dar el usuario
        public Vector3 dirDeDisparo;
        public Vector3 DirDeDisparo
        {
            get { return dirDeDisparo; }
            set { dirDeDisparo = value; }

        }
        public float instanteDeDisparo;
        public float InstanteDeDisparo
        {
            set { this.instanteDeDisparo = value; }
            get { return this.instanteDeDisparo; }
        }
        public Colisionador colisionador;
        public Vector3 velocidadInicial;
        public Vector3 velociadActual;
        public TgcArrow vectoVelActual = new TgcArrow();
        public float alfa;
        public float Alfa//lo defino acá para no tener que acer sqrt() cada vez que rebota un disparo
        {
            get { return alfa; }
            set { alfa = value; }
        }
        

        public float aceleracionInicial()
        {
            float aceleracionInicialDisparo;
            aceleracionInicialDisparo = fuerzaDeLanzamiento / colisionador.Masa;//fuerzaDeLanzamiento / colisionador.Masa;           
            return aceleracionInicialDisparo;
        }
        public float moduloDeVelInicialDisparoAlCuadrado()
        {
            float velInicialDisparoAl2;
            velInicialDisparoAl2 = 2 * aceleracionInicial() * recorridoLanzamiento;//recorridoLanzamiento;//sacado de la formula física: Vf^2=Vi^2+2*a*desplazamiento
            return velInicialDisparoAl2;
        }
        // el vector velocidad inicial(que es (velInicialX,velInicialY,velInicialZ) ) será igual al vector direccion de disparo "dirDeDisparo" porque el disparo serà en esa
        // direccion. Pero la norma es diferente ya que esta norma será determinada por el calculo según la aceleracion provocada por la fuerza de disparo, entonces tengo que 
        // averiguar las velocidades iniciales en cada eje dado ese modulo de velocidad inicial. como sé que el vector velocidad inicial es = alfa*vector direccion, entonces
        // voy a averiguar ese alfa para despues calcular cada componente del vector vel inicial multiplicando alfa * componente del vector direccion de disparo
        //calculo el alfa
        public void calcularAlfa()
        {
            double valor;
            float valorAux;
            valorAux = moduloDeVelInicialDisparoAlCuadrado() / (dirDeDisparo.X * dirDeDisparo.X + dirDeDisparo.Y * dirDeDisparo.Y + dirDeDisparo.Z * dirDeDisparo.Z);
            valor = Math.Sqrt((double)valorAux);
            this.Alfa = (float)valor;
        }
        public void setearVelocidadInicial()
        {
            if (this.Alfa < 0.1)
            {
                this.Alfa = 0;
            }
            this.velocidadInicial.X = this.Alfa * this.DirDeDisparo.X;
            this.velocidadInicial.Y = this.Alfa * this.DirDeDisparo.Y;
            this.velocidadInicial.Z = this.Alfa * this.DirDeDisparo.Z;
        }
        public Vector3 posicionDelDisparo(float tiempo)
        {
            float difTiempo = tiempo - this.InstanteDeDisparo;
            if (this.Alfa == 0) { difTiempo = 0; }
            this.velociadActual.X = this.velocidadInicial.X;
            this.velociadActual.Z = this.velocidadInicial.Z;
            this.velociadActual.Y = (-9.8f) * difTiempo + this.velocidadInicial.Y;
            Vector3 pos = new Vector3(0, 0, 0);
            pos.Z = this.colisionador.PosicionInicial.Z + this.velocidadInicial.Z * difTiempo;
            pos.X = this.colisionador.PosicionInicial.X + this.velocidadInicial.X * difTiempo;
            pos.Y = this.colisionador.PosicionInicial.Y + this.velocidadInicial.Y * difTiempo - 0.5f * 9.8f * difTiempo * difTiempo;
            return pos;
        }
        public Disparo(Colisionador colisionador, Vector3 direccion, float instante)
        {
            this.InstanteDeDisparo = instante;
            this.colisionador = colisionador;
            this.DirDeDisparo = direccion;
            this.calcularAlfa();
            this.setearVelocidadInicial();
        }
        public void renderizar(float tiempo)
        {
            Vector3 posicion = this.posicionDelDisparo(tiempo);
            this.colisionador.PosicionActual = posicion;
            if ((bool)GuiController.Instance.Modifiers["vectoVel"])
            {
                this.vectoVelActual.PStart = posicion;
                this.vectoVelActual.PEnd = this.velociadActual + posicion;
                this.vectoVelActual.updateValues();
                this.vectoVelActual.render();
            }
            else { }

            //if (this.colisionador.conCamara)
            //{
                if (colisionador.RutaDelMesh == "ModelosPropios\\bala.X")
                {
                    //Vector3 ejeRotacion = this.DirDeDisparo-colisionador.PosicionActual;
                    this.colisionador.renderizar(this.DirDeDisparo,this.velociadActual);//this.DirDeDisparo );
                }
                else
                {
                    Vector3 ejeRotacion = Vector3.Cross(new Vector3(0, 10, 0), this.DirDeDisparo);
                    this.colisionador.renderizar(ejeRotacion,this.velociadActual);//this.DirDeDisparo);
                }
            //}
            //else 
            //{
            //    //this.colisionador.renderizar();
            //    Vector3 ejeRotacion = Vector3.Cross(new Vector3(0, 10, 0), this.DirDeDisparo);
            //    this.colisionador.renderizar(ejeRotacion);
            //}

        }
    }
}
