/// <binding />
var gulp = require('gulp');
var browserSync = require('browser-sync').create();
var cache = require('gulp-cache');
var concat = require('gulp-concat');
var sass = require('gulp-sass');
var sourcemaps = require('gulp-sourcemaps');
var uglify = require('gulp-uglify');
var uglifycss = require('gulp-uglifycss');
var gutil = require('gulp-util');

var webroot = "./wwwroot/";
var paths = {
    js: webroot + "js/",
    css: webroot + "css/"
};

// Task clear cache
gulp.task('clearCache', function () {
    cache.clearAll();
});

// Task browser sync 
gulp.task('browserSync', function () {
    browserSync.init({
        proxy: 'http://localhost:59769/',
        host: 'http://localhost:59769/',
        open: 'external',
        port: 3000
    });
});

// Task compile javascript library files
gulp.task('lib-js', function () {
    return gulp.src(['./wwwroot/lib/js/jquery.min.js',
        './wwwroot/lib/js/popper.min.js',
        './wwwroot/lib/js/bootstrap-material-design.min.js',
        './wwwroot/lib/js/perfect-scrollbar.jquery.min.js',
        './wwwroot/lib/js/moment.min.js',
        './wwwroot/lib/js/sweetalert2.js',
        './wwwroot/lib/js/jquery.validate.min.js',
        './wwwroot/lib/js/jquery.bootstrap-wizard.js',
        './wwwroot/lib/js/bootstrap-selectpicker.js',
        './wwwroot/lib/js/bootstrap-datetimepicker.min.js',
        './wwwroot/lib/js/jquery.dataTables.min.js',
        './wwwroot/lib/js/bootstrap-tagsinput.js',
        './wwwroot/lib/js/jasny-bootstrap.min.js',
        './wwwroot/lib/js/fullcalendar.min.js',
        './wwwroot/lib/js/jquery-jvectormap.js',
        './wwwroot/lib/js/nouislider.min.js',
        './wwwroot/lib/js/core.js',
        './wwwroot/lib/js/arrive.min.js',
        './wwwroot/lib/js/buttons.js',
        './wwwroot/lib/js/chartist.min.js',
        './wwwroot/lib/js/bootstrap-notify.js',
        './wwwroot/lib/js/material-dashboard.min.js'])
        .pipe(concat('plugin-dist.js'))
        .pipe(uglify())
        .on('error', function (err) { gutil.log(gutil.colors.red('[Error]'), err.toString()); })
        .pipe(gulp.dest(paths.js));
        // .pipe(browserSync.reload({
        //     stream: true
        // }));
});


// Task compile css library files
gulp.task('lib-css', function () {
    return gulp.src(['./wwwroot/lib/css/material-dashboard.min.css',
        './wwwroot/lib/css/demo.css',
        './wwwroot/lib/css/font-awesome.css',
        './wwwroot/lib/css/google-roboto-300-700.css'])
        .pipe(concat('lib.css'))
        .pipe(uglifycss())
        .on('error', function (err) { gutil.log(gutil.colors.red('[Error]'), err.toString()); })
        .pipe(gulp.dest(paths.css));
        // .pipe(browserSync.reload({
        //     stream: true
        // }));
});

// Task lib combie 2 task above
gulp.task("lib", ["lib-js", "lib-css"]);

// Task compile javascript files
gulp.task('js', function () {
    return gulp.src([paths.js + "**/*.js", "!" + paths.js + "script-dist.js", "!" + paths.js + "plugin-dist.js"])
        .pipe(sourcemaps.init())
        .pipe(concat('script-dist.js'))
        .pipe(uglify())
        .on('error', function (err) { gutil.log(gutil.colors.red('[Error]'), err.toString()); })
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(paths.js));
        // .pipe(browserSync.reload({
        //     stream: true
        // }));
});

// Task compile scss files
gulp.task('sass', function () {
    gulp.src(paths.css + "style.scss")
        .pipe(sourcemaps.init())
        .pipe(sass({ outputStyle: 'compressed' }).on('error', sass.logError))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(paths.css))
        .pipe(browserSync.reload({
            stream: true
        }));
});

gulp.task('bs-reload', function () {
    browserSync.reload();
});

//Watch task
gulp.task('watch', ['browserSync', 'js', 'sass'], function () {
    gulp.watch(paths.js + "**/*.js", ['js']);
    gulp.watch(paths.css + '**/*.scss', ['sass']);
    // gulp.watch('./Views/**/*.cshtml', ['bs-reload']);
});

gulp.task('default', ['watch']);

