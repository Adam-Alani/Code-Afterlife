<script>
	import Home from './components/Home.svelte';
	import About from './components/About.svelte';
	import DevLog from './components/DevLog.svelte';
	import Team from './components/Team.svelte'

	function scrollIntoView(id) {
		document.getElementById(id).scrollIntoView({behavior: 'smooth'});
	}
</script>

<main>
	<div class="flex h-screen ">
		<div class="flex flex-1 flex-row">
			<nav class="fixed flex min-h-full z-20 top-0 flex-none text-white justify-between bg-gray-900">
				<div class="px-4 my-auto">
					<ul class="mx-2"  >
						<li>
							<!-- svelte-ignore a11y-invalid-attribute -->
							<a on:click={()=> {scrollIntoView('about')}}>
							<svg class="nav-button" width="32" height="32" xmlns="http://www.w3.org/2000/svg" fill="white" viewBox="0 0 20 20"><path d="M10.707 2.293a1 1 0 00-1.414 0l-7 7a1 1 0 001.414 1.414L4 10.414V17a1 1 0 001 1h2a1 1 0 001-1v-2a1 1 0 011-1h2a1 1 0 011 1v2a1 1 0 001 1h2a1 1 0 001-1v-6.586l.293.293a1 1 0 001.414-1.414l-7-7z"/></svg>
							</a>
						</li>

						<li class="my-12">
							<!-- svelte-ignore a11y-missing-attribute -->
							<a on:click={()=> {scrollIntoView('log')}}>
								<svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="white" class="bi bi-newspaper nav-button" viewBox="0 0 16 16"><path d="M0 2.5A1.5 1.5 0 0 1 1.5 1h11A1.5 1.5 0 0 1 14 2.5v10.528c0 .3-.05.654-.238.972h.738a.5.5 0 0 0 .5-.5v-9a.5.5 0 0 1 1 0v9a1.5 1.5 0 0 1-1.5 1.5H1.497A1.497 1.497 0 0 1 0 13.5v-11zM12 14c.37 0 .654-.211.853-.441.092-.106.147-.279.147-.531V2.5a.5.5 0 0 0-.5-.5h-11a.5.5 0 0 0-.5.5v11c0 .278.223.5.497.5H12z"/><path d="M2 3h10v2H2V3zm0 3h4v3H2V6zm0 4h4v1H2v-1zm0 2h4v1H2v-1zm5-6h2v1H7V6zm3 0h2v1h-2V6zM7 8h2v1H7V8zm3 0h2v1h-2V8zm-3 2h2v1H7v-1zm3 0h2v1h-2v-1zm-3 2h2v1H7v-1zm3 0h2v1h-2v-1z"/></svg>
							</a>
						</li>
						<li class="my-12">
							<!-- svelte-ignore a11y-missing-attribute -->
							<a on:click={()=> {scrollIntoView('team')}}>
								<svg class="nav-button" width="32" height="32" xmlns="http://www.w3.org/2000/svg" fill="white" viewBox="0 0 20 20"><path d="M13 6a3 3 0 11-6 0 3 3 0 016 0zM18 8a2 2 0 11-4 0 2 2 0 014 0zM14 15a4 4 0 00-8 0v3h8v-3zM6 8a2 2 0 11-4 0 2 2 0 014 0zM16 18v-3a5.972 5.972 0 00-.75-2.906A3.005 3.005 0 0119 15v3h-3zM4.75 12.094A5.973 5.973 0 004 15v3H1v-3a3 3 0 013.75-2.906z"/></svg>
							</a>
						</li>
						<li class="my-12">
							<!-- svelte-ignore a11y-missing-attribute -->
							<a on:click={()=> {scrollIntoView('download')}}>
								<svg class="nav-button" width="32" height="32" xmlns="http://www.w3.org/2000/svg" fill="none" stroke="white" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 13l-3 3m0 0l-3-3m3 3V8m0 13a9 9 0 110-18 9 9 0 010 18z"/></svg>
							</a>
						</li>
					</ul>
				</div>
			</nav>
			<div class="block flex-grow absolute top-0 right-0 bottom-0 left-20">
				<div id="home" class="h-screen">
					<Home/>
				</div>
				<div id="about">
					<About/>
				</div>
				<div id="log">
					<DevLog/>
				</div>
				<div id="team">
					<Team/>
				</div>
				<div class="flex justify-center terminal" id="download">
					<a href="https://github.com/Adam-Alani/Code-Afterlife/releases/tag/0.1a" class="large green text-white rounded-lg my-4 px-6">Download</a>
				</div>
			</div>
		</div>
	</div>
</main>

<style global>

	.large {
		font-size: 3rem;
	}
	.green {
		background-color: #00ff3f;
	}
	.green:hover {
		background-color: DarkGreen;
	}

	.terminal {
		background-color: #101010;
		filter: drop-shadow(0 0 10px #00ff3f);
	}
	@import 'tailwindcss/base';

	@import 'tailwindcss/components';

	@import 'tailwindcss/utilities';



	::-webkit-scrollbar {
		width: 7px;
		height: 7px;
	}
	::-webkit-scrollbar-button {
		width: 0px;
		height: 0px;
	}
	::-webkit-scrollbar-thumb {
		background: #c2c2c2;
		border: 0px none #ffffff;
		border-radius: 50px;
	}
	::-webkit-scrollbar-thumb:hover {
		background: #dbdbdb;
	}
	::-webkit-scrollbar-thumb:active {
		background: #ffffff;
	}
	::-webkit-scrollbar-track {
		background: #666666;
		border: 0px none #ffffff;
		border-radius: 50px;
	}
	::-webkit-scrollbar-track:hover {
		background: #333333;
	}
	::-webkit-scrollbar-track:active {
		background: #333333;
	}
	::-webkit-scrollbar-corner {
		background: transparent;
	}

	.nav-button:hover {
		fill: cornflowerblue;
		cursor: pointer;
	}

</style>
