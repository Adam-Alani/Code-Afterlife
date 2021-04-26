<div class="relative flex flex-1 terminal ">
    <div class=" centered absolute flex  z-20 mx-0 my-auto left-1/2 right-0 ">
        <div class="text-white mt-8 justify-center flex flex-1 ">
            <img src="images/log.png" class="absolute size justify-center align-middle   "/>
        </div>
    </div>
    <div class=" z-10 mx-0 my-auto">
        <div class="mt-28 mb-28 flex-grow text-white text-center  " >
            <div>
            <h1 class="text-xl underline text-green-500">Update Log . . . : 1/3/2021</h1>
                <div class="mt-2 mx-16">
                    <p>
                        Started work on the main map, along with the test puzzle. With that, we also started working on the basic stylistc elements of the game such as the lights, post processing and isometric style. Mouvement and characters are at their early stages, along with the start of the interpreter for a custom-made language, so far, it is capable of evaluating expressions, variable declaration and manipulation, and contains basic features such as while loops and if statements. Finally, started working on more detailed aspects such as the UI/HUD, multiplayer and a custom turret AI.
                    </p>
                </div>
            </div>

            <div class="mt-8"><h1 class="text-xl underline text-purple-600">Update Log . . . : 26/4/2021</h1>
                <div class="mt-2 mx-16">
                    <p>Finished the first level, with it comes the terminal and an IDE with syntax highlighting autoindentation, and unit testing. The IDE uses our own interpreted language Babylon.  now contains the +=, -=, *= and /= operators, for loops, strings floats and lists, and functions.  For the first map, we used 2 terminals that activate/deactivate doors and turrets. We also now have our main character design, a bit more sound design (for the terminal, turret and background music). Finally, multiplayer is now 100% done.</p>
                </div>
            </div>
            <div class="mt-8"><h1 class="text-xl underline text-yellow-200">Update Log 3</h1>
                <div class="mt-2 mx-16">
                    <p>Coming soon</p>
                </div>
            </div>
            <div class="mt-8"><h1 class="text-xl underline text-blue-500">Update Log 4</h1>
                <div class="mt-2 mx-16">
                    <p>Coming soon</p>
                </div>
            </div>
        </div>
    </div>
</div>






<style>

    .terminal {
        background-color: #101010;
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
