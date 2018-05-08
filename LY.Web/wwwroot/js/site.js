; var ly = {
    common: { }
}

; (function (exports) {
    exports.ajax = function(options) {
        var sucessAction = options.success || function () { }
        $.extend(options, {
            beforeSend: function (xhr) {
                if (options.isShow || options.isShow == undefined) {
                    showLoading()
                }
            },
            success: function (obj) {
                hideLoading()
                return sucessAction(obj)
            },
            error: function (jqXhr, status, error) {
                hideLoading()
                alert('请求接口失败');
            }
        })

        function showLoading() {
            if ($('#lyLoading').length === 0) {
                $('body').append('<div id="lyLoading" style="display:none">' +
                    '<div class="loading">loading......</div>' +
                    '<div class="screen-mask"></div>' +
                    '</div>')
            }
            $('#lyLoading').show()
        }

        function hideLoading() {
            if ($('#lyLoading').length !== 0) {
                $('#lyLoading').hide()
            }
        }

        $.ajax(options)
    }
})(ly.common)

// Write your Javascript code.
Vue.component("ly-table", {
    props: ["columns", "url"],
    template: '<table class="table table-bordered table-hover">\
                  <thead><tr><th v-for="column in columns">{{column.title}}</th></tr></thead>\
                  <tbody><tr v-for="row in rows"><td v-for="column in columns">{{row[column.name]}}</td></tr></tbody>\
              </table>',
    computed: {
        rows: function () {
            return this.getData();
        }
    },
    methods: {
        reload111: function () {
            //rows = getData()
        },
        getData: function () {
            var result = [];
            ly.common.ajax({
                url: this.url,
                async: false,
                data: {},
                success: function (data) {
                    result = data;
                }
            });
            return result;
        }
    }
});
