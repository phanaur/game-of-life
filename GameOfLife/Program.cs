using Raylib_cs;
using System.Numerics;
namespace GameOfLife;

class Program
{
    private static void Main()
    {
        // Configuración de las celdas
        const int gridAncho = 100; // Ancho en celdas del grid
        const int gridAlto = 100; // Alto en celdas del grid
        const int tamanoCelda = 8; // tamaño de cada celda
        const int espaciado = 1; // espaciado entre celdas

        // Tamaño total del grid
        const int gridTotalAncho = gridAncho * tamanoCelda + (gridAncho - 1) * espaciado;
        const int gridTotalAlto = gridAlto * tamanoCelda + (gridAlto - 1) * espaciado;

        // Tamaño de la ventana (para que sea más grande que el grid)
        const int margen = 100;
        int ventanaAncho = gridTotalAncho + margen;
        int ventanaAlto = gridTotalAlto + margen;

        // Offset para centrar el grid
        int offsetX = (ventanaAncho - gridTotalAncho) / 2;
        int offsetY = (ventanaAlto - gridTotalAlto) / 2;

        // Estado del grid (false = muerta, true = viva)
        // bool[,] grid = new bool[gridAncho, gridAlto];

        // Vamos a añadir cambios para que el sistema pinte colores en función de la edad (gemini)
        int[,] grid = new int[gridAncho, gridAlto];
        // Comienzo de la simulación
        bool comienzo = false;

        // Estado futuro del grid
        // bool[, ] gridFuture = new bool[gridAncho, gridAlto];

        // Vamos a añadir cambios para que el sistema pinte colores en función de la edad (gemini)
        int[,] gridFuture = new int[gridAncho, gridAlto];

        // añadimos un "interruptor" para ver o no ver la vida
        bool modoEdad = false;

        // Acumulador de tiempo para reducir la velocidad de la simulación (gemini)
        float acumulador = 0;

        // Dibujamos la ventana
        Raylib.InitWindow(ventanaAncho, ventanaAlto, "Conway's Game of Life");
        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {


            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                comienzo = true;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.S))
            {
                comienzo = false;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.C))
            {
                comienzo = false;
                for (int x = 0; x < gridAncho; x++)
                {
                    for (int y = 0; y < gridAlto; y++)
                    {
                        grid[x, y] = 0;
                    }
                }
            }
            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                Random random = new Random();
                for (int x = 0; x < gridAncho; x++)
                {
                    for (int y = 0; y < gridAlto; y++)
                    {
                        grid[x, y] = random.Next(2);
                    }
                }

                comienzo = true;
            }

            // Introduciendo el glider (gemini)
            if (Raylib.IsKeyPressed(KeyboardKey.G))
            {
                Vector2 mousePos = Raylib.GetMousePosition();

                // Calculamos en qué celda estamos (reutiliza tu lógica de clic)
                int celX = (int)((mousePos.X - offsetX) / (tamanoCelda + espaciado));
                int celY = (int)((mousePos.Y - offsetY) / (tamanoCelda + espaciado));

                // Dibujamos el Glider relativo a celX y celY
                grid[(celX + gridAncho + 1) % gridAncho, (celY + gridAlto + 0) % gridAlto] = 1;
                grid[(celX + gridAncho + 2) % gridAncho, (celY + gridAlto + 1) % gridAlto] = 1;
                grid[(celX + gridAncho + 0) % gridAncho, (celY + gridAlto + 2) % gridAlto] = 1;
                grid[(celX + gridAncho + 1) % gridAncho, (celY + gridAlto + 2) % gridAlto] = 1;
                grid[(celX + gridAncho + 2) % gridAncho, (celY + gridAlto + 2) % gridAlto] = 1;
            }

            // Introduciendo el glider (gemini)
            if (Raylib.IsKeyPressed(KeyboardKey.F))
            {
                Vector2 mousePos = Raylib.GetMousePosition();

                // Calculamos en qué celda estamos (reutiliza tu lógica de clic)
                int celX = (int)((mousePos.X - offsetX) / (tamanoCelda + espaciado));
                int celY = (int)((mousePos.Y - offsetY) / (tamanoCelda + espaciado));

                // Dibujamos el Glider relativo a celX y celY
                // Fila 0: El techo de la nave
                grid[(celX + gridAncho + 1) % gridAncho, (celY + gridAlto + 0) % gridAlto] = 1;
                grid[(celX + gridAncho + 2) % gridAncho, (celY + gridAlto + 0) % gridAlto] = 1;
                grid[(celX + gridAncho + 3) % gridAncho, (celY + gridAlto + 0) % gridAlto] = 1;
                grid[(celX + gridAncho + 4) % gridAncho, (celY + gridAlto + 0) % gridAlto] = 1;

                // Fila 1: Los laterales
                grid[(celX + gridAncho + 0) % gridAncho, (celY + gridAlto + 1) % gridAlto] = 1;
                grid[(celX + gridAncho + 4) % gridAncho, (celY + gridAlto + 1) % gridAlto] = 1;

                // Fila 2: La base trasera
                grid[(celX + gridAncho + 4) % gridAncho, (celY + gridAlto + 2) % gridAlto] = 1;

                // Fila 3: El morro y la parte inferior
                grid[(celX + gridAncho + 0) % gridAncho, (celY + gridAlto + 3) % gridAlto] = 1;
                grid[(celX + gridAncho + 3) % gridAncho, (celY + gridAlto + 3) % gridAlto] = 1;

            }

            // Modo edad
            if (Raylib.IsKeyPressed(KeyboardKey.V)) modoEdad = !modoEdad;

            if (!comienzo)
            {
                // Si se clica el izquierdo, se activan celdas
                if (Raylib.IsMouseButtonDown(MouseButton.Left))
                {
                    Vector2 mousePos = Raylib.GetMousePosition();

                    // Calcular coordenadas de celda
                    int celX = (int)((mousePos.X - offsetX) / (tamanoCelda + espaciado));
                    int celY = (int)((mousePos.Y - offsetY) / (tamanoCelda + espaciado));

                    // Verificar que está dentro del grid
                    if (celX is >= 0 and < gridAncho && celY is >= 0 and < gridAlto)
                    {
                        grid[celX, celY] = 1;
                    }
                }

                // Si se clica el derecho, se desactivan celdas

                if (Raylib.IsMouseButtonDown(MouseButton.Right))
                {
                    Vector2 mousePos = Raylib.GetMousePosition();

                    int celX = (int)((mousePos.X - offsetX) / (tamanoCelda + espaciado));
                    int celY = (int)((mousePos.Y - offsetY) / (tamanoCelda + espaciado));

                    if (celX is >= 0 and < gridAncho && celY is >= 0 and < gridAlto)
                    {
                        grid[celX, celY] = 0;
                    }
                }
            }
            else if (acumulador >= 0.2f)
            {
                acumulador = 0;
                for (int x = 0; x < gridAncho; x++)
                {
                    for (int y = 0; y < gridAlto; y++)
                    {
                        int vecinosVivos = 0;
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                // Esta condicion establece paredes "físicas"
                                //if (x + i is >= 0 and < gridAncho && y + j is >= 0 and < gridAlto && (i != 0 || j != 0))
                                //{
                                //    if (grid[x + i, y + j]) vecinosVivos++;
                                //}

                                // Sin embargo, haremos paredes "toroidales", es decir, paredes infinitas.
                                // Si algo desaparece por la derecha saldrá por la izquierda y viceversa.

                                // Hecho con gemini
                                if (i == 0 && j == 0) continue;

                                // Usando el "efecto donut" calculamos el vecino
                                // Sumamos gridAncho y gridAlto antes del módulo para evitar números negativos
                                int vecinoX = (x + i + gridAncho) % gridAncho;
                                int vecinoY = (y + j + gridAlto) % gridAlto;

                                if (grid[vecinoX, vecinoY] > 0) vecinosVivos++;

                                // Actualización con edad
                                if (grid[x, y] > 0 && (vecinosVivos == 2 || vecinosVivos == 3)) {
                                    gridFuture[x, y] = grid[x, y] + 1; // Envejece
                                } else if (grid[x, y] == 0 && vecinosVivos == 3) {
                                    gridFuture[x, y] = 1; // Nace
                                } else {
                                    gridFuture[x, y] = 0; // Muere
                                }
                            }
                        }


                        // Quitamos el switch para adaptarlo a la vida
                        //gridFuture[x, y] = vecinosVivos switch
                        //{
                        //    2 => grid[x, y],
                        //    3 => true,
                        //    _ => false
                        //};


                        /*
                         * Es lo mismo que escribir
                         * switch (vecinosVivos)
                         *  case 2:
                         *      gridFuture[x, y] = grid{x, y];
                         *      break;
                         *  case 3:
                         *      gridFuture[x, y] = true;
                         *      break;
                         *  default:
                         *      gridFuture[x, y] = false;
                         *      break;
                         */
                    }
                }
                grid = (int[,])gridFuture.Clone();
            }

            acumulador += Raylib.GetFrameTime();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            // Para dibujar el grid, hacemos dos bucles for "nesteados" de la siguiente forma:
            // posX siempre será el offsetX más una cantidad, x, que se irá incrementando
            // Lo mismo para posY

            for (int x = 0; x < gridAncho; x++)
            {
                for(int y = 0; y < gridAlto; y++)
                {
                    // Cálculo de las posiciones de cada celda
                    int posX = offsetX + x * (tamanoCelda + espaciado);
                    int posY = offsetY + y * (tamanoCelda + espaciado);

                    // Dibujamos Color según el estado de la celda
                    Color color;
                    if (grid[x, y] > 0) {
                        if (modoEdad) {
                            // El color cambia según la edad.
                            // Ejemplo: de azul (joven) a rojo (viejo)
                            float hue = Math.Max(0, 240 - (grid[x, y] * 2));
                            color = Raylib.ColorFromHSV(hue, 0.8f, 1.0f);
                        } else {
                            color = Color.White;
                        }
                    } else {
                        color = Color.DarkGray;
                    }

                    // Dibujamos la celda como un rectángulo de lados iguales a tamanoCelda
                    Raylib.DrawRectangle(posX, posY, tamanoCelda, tamanoCelda, color);
                }
            }

            Raylib.DrawText(
                text:"Game of Life - Click izq: activa - Click der: desactiva - C: borrar - R: aleatorio - V: modoEdad",
                posX:10,
                posY:10,
                fontSize:20,
                Color.White);
            Raylib.DrawText(
                text:"G: introduce un glider- F: introduce Lightweight spaceship - Space: reproduce - S: pausa",
                posX:10,
                posY:ventanaAlto - margen / 3,
                fontSize:20,
                Color.White);
            Raylib.EndDrawing();

        }
    }
}
