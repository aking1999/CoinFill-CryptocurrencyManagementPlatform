$(document).ready(function () {

    Offer();

    $('.particle-pen').click(function () {
        $('#sparkle-modal').modal('hide');
    })

    function Offer() {
        $('[data-demo-account="true"]').click(function () {
            $('.modal').modal('hide');
            $('.swal-container').remove();
            setTimeout(function () {
                $('#sparkle-modal').modal();
            }, 50)

            const backdropClone = $('#backdrop-round').html();
            const svgClone = $('.particle-pen .svg-sign-in').html();

            setInterval(function () {
                $('#backdrop-round').html('').html(backdropClone);
                $('.particle-pen .svg-sign-in').html('').html(svgClone);
            }, 4000)
        })

        if (window.location.href.indexOf('demo') > -1 && localStorage.getItem('swalAppearedOnce') === null) {
            Swal.fire({
                title: 'This is a demo account',
                text: 'This demo account is only meant to give you a glimpse of how the app looks and functions. To perform real operations, you need to sign in.',
                icon: 'info',
                confirmButtonColor: "#27bcfd",
                confirmButtonText: 'Alright'
            })

            localStorage.setItem('swalAppearedOnce', 'true');
        }
    }
});