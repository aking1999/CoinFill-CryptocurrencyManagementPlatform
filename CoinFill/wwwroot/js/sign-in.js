const RANDOM = (min, max) => Math.floor(Math.random() * (max - min + 1) + min)
document.querySelectorAll('.particle').forEach(P => {
	P.setAttribute('style', `
		--x: 50;
		--y: 70;
		--duration: ${RANDOM(6, 20)};
		--delay: ${RANDOM(1, 10)};
		--alpha: 1;
		--origin-x: ${Math.random() > 0.5 ? RANDOM(300, 800) * -1 : RANDOM(300, 800)}%;
		--origin-y: ${Math.random() > 0.5 ? RANDOM(300, 800) * -1 : RANDOM(300, 800)}%;
		--size: ${RANDOM(40, 90) / 100};
	`)
})