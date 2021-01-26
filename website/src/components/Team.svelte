<div class=" terminal">
    <div class="   flex  z-20 mx-0 my-auto left-1/2 right-0 ">
        <div class=" mt-8 justify-center flex flex-1 ">
            <img src="images/about.png" class=" size justify-around align-middle  "/>
        </div>
    </div>
    <div class="grid grid-cols-1 lg:grid-cols-2  xl:grid-cols-4 py-12 mx-12  ">

                <div class="align-top inline-block text-center px-8 items-center justify-center px-8 flex flex-col">
                    <img src="images/raph.png" class="image" width="250px" height="auto" />
                    <h1 class="mt-2 text-pink-500 big underline te">Raphael</h1>
                    <span class="text-white med">He does this</span>
                </div>
                <div class="align-top inline-block text-center px-8  items-center justify-center px-8 flex flex-col">
                    <img src="images/theo.png" class="image" width="250px" height="auto" />
                    <h1 class="mt-2 text-pink-500 big underline">Theo</h1>
                    <span class="text-white med">Him too</span>
                </div>
                <div class="align-top inline-block text-center px-8 items-center justify-center px-8 flex flex-col">
                    <img src="images/adam.png" class="image" width="250px" height="auto"/>
                    <h1 class="mt-2 text-pink-500 big underline">Adam</h1>
                    <span class="text-white med">Same here</span>
                </div>
                <div class="align-top inline-block text-center px-8 items-center justify-center px-8 flex flex-col">
                    <img src="images/raph.png" class="image" width="250px" height="auto"/>
                    <h1 class="mt-2 text-pink-500 big underline">Ivan</h1>
                    <span class="text-white med">Gros glandeur</span>
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
    .image {
        border-radius: 100%;
        filter: drop-shadow(-10px 3px 0px #DB2777);
    }
    .card {
        flex: 0 1 24%;
    }
    .terminal {
        background-color: #00d9ff;
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
