/// <binding ProjectOpened='sass-watch' />
var gulp = require('gulp'),
    concat = require('gulp-concat'),
    sass = require('gulp-dart-sass'),
    cleancss = require('gulp-clean-css'),
    rename = require('gulp-rename'),
    stripComments = require('gulp-strip-comments'),
    merge = require('merge2');

//#region Options
var options = {
    css: {
        libFiles: [
            '@mdi/font/css/materialdesignicons.css'
        ],
        workingDirectory: 'node_modules',
        sassSource: 'Sass/site.scss',
        sassFiles: 'Sass/**/*.scss',
        output: 'bundle.css',
        dest: 'wwwroot/lib'
    },
    fonts: {
        files: [
            'node_modules/@mdi/font/fonts/*.*'
        ],
        dest: 'wwwroot/fonts'
    }
};
//#endregion

//#region Tasks
gulp.task('bundle-CSS', function () {
    var libCSS = gulp.src(options.css.libFiles, { cwd: options.css.workingDirectory });

    var siteCSS = gulp.src(options.css.sassSource)
        .pipe(sass({
            errLogToConsole: true
        }).on('error', sass.logError));

    return merge(libCSS, siteCSS)
        .pipe(concat(options.css.output))
        .pipe(gulp.dest(options.css.dest))
        .pipe(cleancss())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(options.css.dest));
});

gulp.task('copy-fonts', function () {
    return gulp.src(options.fonts.files)
        .pipe(gulp.dest(options.fonts.dest));
});

gulp.task('sass-watch', function () {
    gulp.watch(options.css.sassFiles, gulp.parallel('bundle-CSS'));
});

gulp.task('default', gulp.parallel('bundle-CSS', 'copy-fonts'));
//#endregion