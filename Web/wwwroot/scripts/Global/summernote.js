

function initSummernote() {
    let summernoteDiv = $('div.summernote');

    if (!summernoteDiv.attr('initialised-sn')) {

        summernoteDiv.attr('initialised-sn', true);

        let summernoteContainer = summernoteDiv.parent().closest('div');
        let summernoteContentInput = summernoteContainer.find('input.summernote-content');
        let summernoteInitialContent = summernoteContentInput.val() ?? '';

        summernoteDiv.summernote({
            placeholder: '',
            tabsize: 2,
            height: 100,
            toolbar: [
                /*['style', ['style']],*/
                ['font', ['bold', 'underline', 'clear']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                /*['table', ['table']],*/
                ['insert', ['link',]], //'picture', 'video']],
                /*['view', ['fullscreen', 'codeview', 'help']]*/
            ]
        });

        summernoteDiv.summernote('code', summernoteInitialContent);

        let summernoteEditor = summernoteContainer.find('.note-editable.card-block');

        summernoteEditor.on('change, blur', function () {
            let currentSummernoteContent = summernoteDiv.summernote('code');
            summernoteContentInput.val(currentSummernoteContent);
            summernoteContentInput.attr('value', currentSummernoteContent); // make sure!
        });
    }
}