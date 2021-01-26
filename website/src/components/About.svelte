<script>
    import AboutContent from './AboutContent.svelte'
</script>


<div class="image-bg bg-no-repeat object-cover">
    <div class="centered absolute flex flex-grow z-20 mx-0 my-auto left-1/2 right-0 ">
       <div class="text-white mt-8 justify-center flex flex-1 ">
            <img src="images/Code to survive.png" class="absolute size justify-center align-middle  "/>
       </div>
    </div>
    <div class="flex items-center justify-center"  >
        <div class="my-16 flex flex-grow " >
            <AboutContent/>
        </div>
    </div>


</div>



<style>
    .image-bg {
        background: url(/images/background2.png);
        background-size: cover;

    }
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
