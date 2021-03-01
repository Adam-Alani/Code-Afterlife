<script>


</script>


<img src="images/background.png" class=" blur top-0 left-0 bg-fixed w-full h-full bg-no-repeat object-cover"/>
<div class="absolute left-1/2 top-1/3">
    <div class="relative negleft">
        <img src="images/logo.png" class=" z-10 max-w-1/2 h-auto bg-fixed logo"/>
    </div>
</div>


<style>
    .blur {
        filter: blur(5px)
    }

    .negleft {
        left:-50%;
        top:-33%;
    }
    @import 'tailwindcss/base';
    @import 'tailwindcss/components';
    @import 'tailwindcss/utilities';

    @keyframes grow_and_rotate {
        0% {
            transform: rotate(-2.5deg);
        }

        100% {
            transform: rotate(2.5deg);
        }
    }

    .logo {
        animation: 1s ease-in-out infinite alternate grow_and_rotate;
    }
</style>
