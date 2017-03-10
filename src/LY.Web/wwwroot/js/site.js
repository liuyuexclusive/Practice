// Write your Javascript code.
Vue.component("ly-table", {
    props: ["columns", "url"],
    template: '<table class="table table-bordered table-hover">\
                        <thead><tr><th v-for="column in columns">{{column.title}}</th></tr></thead>\
                        <tbody><tr v-for="row in rows"><td v-for="column in columns">{{row[column.name]}}</td></tr></tbody>\
                       </table>',
    computed: {
        rows: function () {
            var result = [];
            $.ajax({
                url: this.url,
                async: false,
                data: {},
                success: function (data) {
                    console.log(data);
                    result = data;
                }
            });
            return result;
        }
    }
});
