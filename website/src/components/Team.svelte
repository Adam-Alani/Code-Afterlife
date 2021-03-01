<div class=" terminal ">
    <div class="   flex  z-20 mx-0 my-auto left-1/2 right-0 ">
        <div class=" mt-8 justify-center flex flex-1 ">
            <img src="images/about.png" class=" size justify-around align-middle  "/>
        </div>
    </div>
    <div class="grid gap-5 grid-cols-1 lg:grid-cols-2  xl:grid-cols-4 py-12 mx-12  ">

                <div class=" inline-block  px-8 items-center justify-center px-8  card rounded-lg shadow-lg">
                    <h1 class="mt-2 text-pink-500 big underline te text-center ">Raphael</h1>
                    <img src="images/raph.png" class="image" width="250px" height="auto" />
                    <span class="text-white med">Works primarily on the Babylon Interpreter</span>
                </div>
                <div class=" inline-block  px-8  items-center justify-center px-8 card rounded-lg hadow-lg">
                    <h1 class="mt-2 text-pink-500 big underline text-center">Theo</h1>
                    <img src="images/theo.png" class="image" width="250px" height="auto" />
                    
                    <span class="text-white med">Works on game design, controls and characters</span>
                </div>
                <div class=" inline-block px-8 items-center justify-center px-8 card rounded-lg shadow-lg">
                    <h1 class="mt-2 text-pink-500 big underline text-center">Adam</h1>
                    <img src="images/adam.png" class="image" width="250px" height="auto"/>
                    
                    <span class="text-white med">Works on the graphics and the IDE/Terminal</span>
                </div>
                <div class=" inline-block px-8 items-center justify-center px-8 card rounded-lg shadow-lg">
                    <h1 class="mt-2 text-pink-500 big underline text-center">Ivan</h1>
                    <img src="images/ivan.png" class="image" width="250px" height="auto"/>
                    <span class="text-white med">Works on the multiplayer and AI</span>
                </div>
            </div>
    </div>





<style>
    .wrapper {
        width: 75%;
    }
    .box {

        display: flex;
        flex-wrap: wrap;
        margin:-10px;
    }

    .image {
        @apply shadow-lg;
        border-radius: 2%;
    }
    .card {
        background-color: #0f0917;
        padding-bottom:1rem;

    }

    .big {
        font-size: 3rem;
    }
    .med {
        font-size: 1.5rem;
    }
    .box>* {
        flex: 1 1 160px;
        margin: 10px;
    }

    .card {
        flex: 0 1 24%;
    }
    .terminal {
        background-color: #211231;
        filter: drop-shadow(0 0 10px #DB2777);
    }
    .size {
        width: 250px;
        height: auto;
    }
    .centered {
        transform: translate(-50%, 0);
    }

    @import 'tailwindcss/base';
    @import 'tailwindcss/components';
    @import 'tailwindcss/utilities';
</style>
