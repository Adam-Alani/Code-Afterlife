<script>
    import AboutContent from './AboutContent.svelte'
</script>


<div class="relative ">
    <div class="centered absolute flex z-20 mx-0 my-auto left-1/2 right-0 ">
       <div class="text-white mt-8 justify-center flex flex-1 ">
            <img src="images/Code to survive.png" class="absolute size justify-center align-middle  "/>
       </div>
    </div>
    <div class="centered absolute flex justify-center z-10 mx-0 my-auto left-1/2 right-0 w-full h-full ">
        <div class="mt-16 flex-grow " >
            <AboutContent/>
        </div>
    </div>

    <img src="images/background2.png" class="top-0 left-0 bg-fixed bg-f w-full h-full bg-no-repeat object-cover"/>


</div>



<style>
    .size {
        width: 250px;
        height: auto;
    }
    @import 'tailwindcss/base';
    @import 'tailwindcss/components';
    @import 'tailwindcss/utilities';
    .centered {
        transform: translate(-50%, 0);
    }
</style>