$(document).ready(async function () {
    $('#home-section').removeClass('px-2 px-lg-0');
    $('#nav-container-main').removeClass('container mt-3');
    $('#nav-card-main').addClass('navbar-shadow').removeClass('card mb-3');
    $('#nav-cardbody-main').removeClass('card-body p-0');
    $('#container-main').removeClass('container');
    $('#container-main').addClass('px-2 px-lg-0 bg-glass-colorful');
    $('#footer').addClass('px-2 px-lg-0');

    function AddStakingCssRules() {
        //var style = $('<style id="staking-rules">' +
        //    '.staking-img-container { border-top-color: #2ba0ff !important; transition: 0.5s linear; }' +
        //    '.staking-img { color: black; transition: 0.5s linear; }' +
        //    '.staking-img-container { animation: stake-spin 1s linear infinite; }' +
        //    '.staking-img-self { animation: stake-spin-reverse 1s linear infinite; }' +
        //    '@@keyframes stake-spin { from { transform: rotate(0deg); } to { transform: rotate(360deg); } }' +
        //    '@@keyframes stake-spin-reverse { from { transform: rotate(360deg); } to { transform: rotate(0deg); } }' +
        //    '</style>');
        $('head #border-rule').remove();
        var style = $('<style id="border-rule">.staking-img-container { border-top-color: #2ba0ff !important; }</style>');
        $('html > head').append(style);
    }

    function RemoveStakingCssRules() {
        $('head #border-rule').remove();
        var style = $('<style id="border-rule">.staking-img-container { border-top-color: transparent !important; }</style>');
        $('html > head').append(style);
    }

    const stakingsHolder = document.getElementById('stakings-holder');

    const portfolio = $('#portfolio');
    const portfolioArrow = $('#portfolio-arrow');
    let portfolioCurrentAmount = 0;

    const solJs = document.getElementById('sol-staking');
    let solIsInView = false;
    const sol = $(solJs);
    const solPrice = 115;
    let solCurrentAmount = 42;
    let solPercentageToAdd = 0.32;
    const solCurrentAmountEl = $('#sol-amount');
    const solAmountToAddEl = $('#sol-amount-to-add');

    const daiJs = document.getElementById('dai-staking');
    let daiIsInView = false;
    const dai = $(daiJs);
    const daiPrice = 1;
    let daiCurrentAmount = 530;
    let daiPercentageToAdd = 0.35;
    const daiCurrentAmountEl = $('#dai-amount');
    const daiAmountToAddEl = $('#dai-amount-to-add');

    const tiaJs = document.getElementById('tia-staking');
    let tiaIsInView = false;
    const tia = $(tiaJs);
    const tiaPrice = 22;
    let tiaCurrentAmount = 180;
    let tiaPercentageToAdd = 0.32;
    const tiaCurrentAmountEl = $('#tia-amount');
    const tiaAmountToAddEl = $('#tia-amount-to-add');

    const dotJs = document.getElementById('dot-staking');
    let dotIsInView = false;
    const dot = $(dotJs);
    const dotPrice = 9;
    let dotCurrentAmount = 270;
    let dotPercentageToAdd = 0.29;
    const dotCurrentAmountEl = $('#dot-amount');
    const dotAmountToAddEl = $('#dot-amount-to-add');

    const aptJs = document.getElementById('apt-staking');
    let aptIsInView = false;
    const apt = $(aptJs);
    const aptPrice = 11;
    let aptCurrentAmount = 350;
    let aptPercentageToAdd = 0.37;
    const aptCurrentAmountEl = $('#apt-amount');
    const aptAmountToAddEl = $('#apt-amount-to-add');

    const opJs = document.getElementById('op-staking');
    let opIsInView = false;
    const op = $(opJs);
    const opPrice = 5;
    let opCurrentAmount = 230;
    let opPercentageToAdd = 0.37;
    const opCurrentAmountEl = $('#op-amount');
    const opAmountToAddEl = $('#op-amount-to-add');

    setTimeout(function () {
        AddStakingCssRules();
    }, 3800)

    CalculatePortfolio();

    const sleep = (delay) => new Promise((resolve) => setTimeout(resolve, delay))

    ///////////////////////////

    var solObserver = new IntersectionObserver(SolOnIntersection, {
        root: null,
        threshold: .5
    })
    function SolOnIntersection(entries, opts) {
        entries.forEach(entry => {
            if (entry.isIntersecting) solIsInView = true;
            else solIsInView = false;
        })
    }
    solObserver.observe(solJs);

    ///////////////////////////

    var daiObserver = new IntersectionObserver(DaiOnIntersection, {
        root: null,
        threshold: .5
    })
    function DaiOnIntersection(entries, opts) {
        entries.forEach(entry => {
            if (entry.isIntersecting) daiIsInView = true;
            else daiIsInView = false;
        })
    }
    daiObserver.observe(daiJs);

    ///////////////////////////

    var tiaObserver = new IntersectionObserver(TiaOnIntersection, {
        root: null,
        threshold: .5
    })
    function TiaOnIntersection(entries, opts) {
        entries.forEach(entry => {
            if (entry.isIntersecting) tiaIsInView = true;
            else tiaIsInView = false;
        })
    }
    tiaObserver.observe(tiaJs);

    ///////////////////////////

    var dotObserver = new IntersectionObserver(DotOnIntersection, {
        root: null,
        threshold: .5
    })
    function DotOnIntersection(entries, opts) {
        entries.forEach(entry => {
            if (entry.isIntersecting) dotIsInView = true;
            else dotIsInView = false;
        })
    }
    dotObserver.observe(dotJs);

    ///////////////////////////

    var aptObserver = new IntersectionObserver(AptOnIntersection, {
        root: null,
        threshold: .5
    })
    function AptOnIntersection(entries, opts) {
        entries.forEach(entry => {
            if (entry.isIntersecting) aptIsInView = true;
            else aptIsInView = false;
        })
    }
    aptObserver.observe(aptJs);

    ///////////////////////////

    var opObserver = new IntersectionObserver(OpOnIntersection, {
        root: null,
        threshold: .5
    })
    function OpOnIntersection(entries, opts) {
        entries.forEach(entry => {
            if (entry.isIntersecting) opIsInView = true;
            else opIsInView = false;
        })
    }
    opObserver.observe(opJs);

    ///////////////////////////

    setTimeout(async function () {
        let current = 'sol';

        while (true) {
            if (solIsInView || daiIsInView || tiaIsInView ||
                dotIsInView || aptIsInView || opIsInView) {
                if (current == 'sol' && solIsInView) {
                    RemoveStakingSpinner(sol);
                    const solAmountToAdd = Math.round(solCurrentAmount * solPercentageToAdd);
                    solAmountToAddEl.attr('data-countup', `{"startVal":${solAmountToAdd}, "endValue":${0}, "prefix":"+ ", "suffix":" SOL"}`);
                    solCurrentAmountEl.attr('data-countup', `{"startVal":${solCurrentAmount}, "endValue":${solCurrentAmount + solAmountToAdd}, "suffix":" SOL"}`);
                    solCurrentAmount += solAmountToAdd;

                    portfolio.attr('data-countup', `{"startVal":${portfolioCurrentAmount}, "endValue":${portfolioCurrentAmount + solAmountToAdd * solPrice}, "prefix":"$ "}`);
                    portfolioCurrentAmount = portfolioCurrentAmount + solAmountToAdd * solPrice;

                    solAmountToAddEl.html('+ ' + solAmountToAdd + ' SOL').fadeIn('slow', function () {
                        setTimeout(function () {
                            portfolioArrow.css({ opacity: 0.0, visibility: "visible" }).animate({ opacity: 1.0 });

                            document.querySelectorAll('#portfolio[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            document.querySelectorAll('#sol-amount[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            document.querySelectorAll('#sol-amount-to-add[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            setTimeout(function () {
                                solAmountToAddEl.fadeOut('slow', function () {
                                    AddStakingSpinner(sol);
                                    portfolioArrow.css({ opacity: 1, visibility: "visible" }).animate({ opacity: 0 });
                                    portfolio.removeAttr('data-countup');
                                    solCurrentAmountEl.removeAttr('data-countup');
                                    solAmountToAddEl.removeAttr('data-countup');
                                });
                            }, 2000)
                        }, 300)
                    })
                } else if (current == 'dai' && daiIsInView) {
                    RemoveStakingSpinner(dai);
                    const daiAmountToAdd = Math.round(daiCurrentAmount * daiPercentageToAdd);
                    daiAmountToAddEl.attr('data-countup', `{"startVal":${daiAmountToAdd}, "endValue":${0}, "prefix":"+ ", "suffix":" DAI"}`);
                    daiCurrentAmountEl.attr('data-countup', `{"startVal":${daiCurrentAmount}, "endValue":${daiCurrentAmount + daiAmountToAdd}, "suffix":" DAI"}`);
                    daiCurrentAmount += daiAmountToAdd;

                    portfolio.attr('data-countup', `{"startVal":${portfolioCurrentAmount}, "endValue":${portfolioCurrentAmount + daiAmountToAdd * daiPrice}, "prefix":"$ "}`);
                    portfolioCurrentAmount = portfolioCurrentAmount + daiAmountToAdd * daiPrice;

                    daiAmountToAddEl.html('+ ' + daiAmountToAdd + ' DAI').fadeIn('slow', function () {
                        setTimeout(function () {
                            portfolioArrow.css({ opacity: 0.0, visibility: "visible" }).animate({ opacity: 1.0 });

                            document.querySelectorAll('#portfolio[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            document.querySelectorAll('#dai-amount[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            document.querySelectorAll('#dai-amount-to-add[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            setTimeout(function () {
                                daiAmountToAddEl.fadeOut('slow', function () {
                                    AddStakingSpinner(dai);
                                    portfolioArrow.css({ opacity: 1, visibility: "visible" }).animate({ opacity: 0 });
                                    portfolio.removeAttr('data-countup');
                                    daiCurrentAmountEl.removeAttr('data-countup');
                                    daiAmountToAddEl.removeAttr('data-countup');
                                });
                            }, 2000)
                        }, 300)
                    })
                } else if (current == 'tia' && tiaIsInView) {
                    RemoveStakingSpinner(tia);
                    const tiaAmountToAdd = Math.round(tiaCurrentAmount * tiaPercentageToAdd);
                    tiaAmountToAddEl.attr('data-countup', `{"startVal":${tiaAmountToAdd}, "endValue":${0}, "prefix":"+ ", "suffix":" TIA"}`);
                    tiaCurrentAmountEl.attr('data-countup', `{"startVal":${tiaCurrentAmount}, "endValue":${tiaCurrentAmount + tiaAmountToAdd}, "suffix":" TIA"}`);
                    tiaCurrentAmount += tiaAmountToAdd;

                    portfolio.attr('data-countup', `{"startVal":${portfolioCurrentAmount}, "endValue":${portfolioCurrentAmount + tiaAmountToAdd * tiaPrice}, "prefix":"$ "}`);
                    portfolioCurrentAmount = portfolioCurrentAmount + tiaAmountToAdd * tiaPrice;

                    tiaAmountToAddEl.html('+ ' + tiaAmountToAdd + ' TIA').fadeIn('slow', function () {
                        setTimeout(function () {
                            portfolioArrow.css({ opacity: 0.0, visibility: "visible" }).animate({ opacity: 1.0 });

                            document.querySelectorAll('#portfolio[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            document.querySelectorAll('#tia-amount[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            document.querySelectorAll('#tia-amount-to-add[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            setTimeout(function () {
                                tiaAmountToAddEl.fadeOut('slow', function () {
                                    AddStakingSpinner(tia);
                                    portfolioArrow.css({ opacity: 1, visibility: "visible" }).animate({ opacity: 0 });
                                    portfolio.removeAttr('data-countup');
                                    tiaCurrentAmountEl.removeAttr('data-countup');
                                    tiaAmountToAddEl.removeAttr('data-countup');
                                });
                            }, 2000)
                        }, 300)
                    })
                } else if (current == 'dot' && dotIsInView) {
                    RemoveStakingSpinner(dot);
                    const dotAmountToAdd = Math.round(dotCurrentAmount * dotPercentageToAdd);
                    dotAmountToAddEl.attr('data-countup', `{"startVal":${dotAmountToAdd}, "endValue":${0}, "prefix":"+ ", "suffix":" DOT"}`);
                    dotCurrentAmountEl.attr('data-countup', `{"startVal":${dotCurrentAmount}, "endValue":${dotCurrentAmount + dotAmountToAdd}, "suffix":" DOT"}`);
                    dotCurrentAmount += dotAmountToAdd;

                    portfolio.attr('data-countup', `{"startVal":${portfolioCurrentAmount}, "endValue":${portfolioCurrentAmount + dotAmountToAdd * dotPrice}, "prefix":"$ "}`);
                    portfolioCurrentAmount = portfolioCurrentAmount + dotAmountToAdd * dotPrice;

                    dotAmountToAddEl.html('+ ' + dotAmountToAdd + ' DOT').fadeIn('slow', function () {
                        setTimeout(function () {
                            portfolioArrow.css({ opacity: 0.0, visibility: "visible" }).animate({ opacity: 1.0 });

                            document.querySelectorAll('#portfolio[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            document.querySelectorAll('#dot-amount[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            document.querySelectorAll('#dot-amount-to-add[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            setTimeout(function () {
                                dotAmountToAddEl.fadeOut('slow', function () {
                                    AddStakingSpinner(dot);
                                    portfolioArrow.css({ opacity: 1, visibility: "visible" }).animate({ opacity: 0 });
                                    portfolio.removeAttr('data-countup');
                                    dotCurrentAmountEl.removeAttr('data-countup');
                                    dotAmountToAddEl.removeAttr('data-countup');
                                });
                            }, 2000)
                        }, 300)
                    })
                } else if (current == 'apt' && aptIsInView) {
                    RemoveStakingSpinner(apt);
                    const aptAmountToAdd = Math.round(aptCurrentAmount * aptPercentageToAdd);
                    aptAmountToAddEl.attr('data-countup', `{"startVal":${aptAmountToAdd}, "endValue":${0}, "prefix":"+ ", "suffix":" APT"}`);
                    aptCurrentAmountEl.attr('data-countup', `{"startVal":${aptCurrentAmount}, "endValue":${aptCurrentAmount + aptAmountToAdd}, "suffix":" APT"}`);
                    aptCurrentAmount += aptAmountToAdd;

                    portfolio.attr('data-countup', `{"startVal":${portfolioCurrentAmount}, "endValue":${portfolioCurrentAmount + aptAmountToAdd * aptPrice}, "prefix":"$ "}`);
                    portfolioCurrentAmount = portfolioCurrentAmount + aptAmountToAdd * aptPrice;

                    aptAmountToAddEl.html('+ ' + aptAmountToAdd + ' APT').fadeIn('slow', function () {
                        setTimeout(function () {
                            portfolioArrow.css({ opacity: 0.0, visibility: "visible" }).animate({ opacity: 1.0 });

                            document.querySelectorAll('#portfolio[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            document.querySelectorAll('#apt-amount[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            document.querySelectorAll('#apt-amount-to-add[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            setTimeout(function () {
                                aptAmountToAddEl.fadeOut('slow', function () {
                                    AddStakingSpinner(apt);
                                    portfolioArrow.css({ opacity: 1, visibility: "visible" }).animate({ opacity: 0 });
                                    portfolio.removeAttr('data-countup');
                                    aptCurrentAmountEl.removeAttr('data-countup');
                                    aptAmountToAddEl.removeAttr('data-countup');
                                });
                            }, 2000)
                        }, 300)
                    })
                } else if (current == 'op' && opIsInView) {
                    RemoveStakingSpinner(op);
                    const opAmountToAdd = Math.round(opCurrentAmount * opPercentageToAdd);
                    opAmountToAddEl.attr('data-countup', `{"startVal":${opAmountToAdd}, "endValue":${0}, "prefix":"+ ", "suffix":" OP"}`);
                    opCurrentAmountEl.attr('data-countup', `{"startVal":${opCurrentAmount}, "endValue":${opCurrentAmount + opAmountToAdd}, "suffix":" OP"}`);
                    opCurrentAmount += opAmountToAdd;

                    portfolio.attr('data-countup', `{"startVal":${portfolioCurrentAmount}, "endValue":${portfolioCurrentAmount + opAmountToAdd * opPrice}, "prefix":"$ "}`);
                    portfolioCurrentAmount = portfolioCurrentAmount + opAmountToAdd * opPrice;

                    opAmountToAddEl.html('+ ' + opAmountToAdd + ' OP').fadeIn('slow', function () {
                        setTimeout(function () {
                            portfolioArrow.css({ opacity: 0.0, visibility: "visible" }).animate({ opacity: 1.0 });

                            document.querySelectorAll('#portfolio[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            document.querySelectorAll('#op-amount[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            document.querySelectorAll('#op-amount-to-add[data-countup]').forEach((node) => {
                                const { endValue, ...options } = JSON.parse($(node).attr('data-countup'));
                                const c = new countUp.CountUp(node, endValue, {
                                    duration: 2,
                                    ...options
                                });
                                if (!c.error) {
                                    c.start();
                                } else {
                                    throw new Error('CountUp error: ' + c.error);
                                }
                            });

                            setTimeout(function () {
                                opAmountToAddEl.fadeOut('slow', function () {
                                    AddStakingSpinner(op);
                                    portfolioArrow.css({ opacity: 1, visibility: "visible" }).animate({ opacity: 0 });
                                    portfolio.removeAttr('data-countup');
                                    opCurrentAmountEl.removeAttr('data-countup');
                                    opAmountToAddEl.removeAttr('data-countup');
                                });
                            }, 2000)
                        }, 300)
                    })
                }

                current = GetNextCrypto(current);
                await sleep(5000);
            } else {
                await sleep(2000);
            }
        }
    }, 5200)

    function numberWithCommas(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    function CalculatePortfolio() {
        portfolioCurrentAmount =
            solPrice * solCurrentAmount +
            daiPrice * daiCurrentAmount +
            tiaPrice * tiaCurrentAmount +
            dotPrice * dotCurrentAmount +
            aptPrice * aptCurrentAmount +
            opPrice * opCurrentAmount;
        portfolio.html('$ ' + numberWithCommas(portfolioCurrentAmount))
    }

    function AddStakingSpinner(element) {
        $(element).attr('style', function (i, s) { return (s || '') + 'border-top-color: var(--primary) !important;' });
    }

    function RemoveStakingSpinner(element) {
        $(element).attr('style', function (i, s) { return (s || '') + 'border-top-color: transparent !important;' });
    }

    function GetNextCrypto(cryptoName) {
        if (cryptoName == 'sol') return 'dai';
        else if (cryptoName == 'dai') return 'tia';
        else if (cryptoName == 'tia') return 'dot';
        else if (cryptoName == 'dot') return 'apt';
        else if (cryptoName == 'apt') return 'op';
        else if (cryptoName == 'op') return 'sol';
    }

    $('#stake-now').click(function () {
        $('html, body').animate({
            scrollTop: $('#cryptos-holder').offset().top
        }, 1000);
    })
})