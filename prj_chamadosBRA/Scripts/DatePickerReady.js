if (!Modernizr.inputtypes.date) {

    $(function () {

        $('.datepicker').datepicker({
            dateFormat: 'dd/mm/yyyy',
            language: 'pt-BR'
        }); //Initialise any date pickers

    });

}