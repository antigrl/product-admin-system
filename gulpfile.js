var gulp       = require('gulp');
var sass       = require('gulp-sass');
var watch      = require('gulp-watch');
var sourcemaps = require('gulp-sourcemaps');

gulp.task('sass', function () {
    gulp.src('./ProductAdministrationSystem/Content/sass/**/*.scss')
        .pipe(sourcemaps.init())
        .pipe(sass( {
          errLogToConsole: true
        }))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest('./ProductAdministrationSystem/Content'));
});

gulp.task('watch', function() {
  gulp.watch('./ProductAdministrationSystem/Content/sass/**/*.scss', ['sass']);
});

gulp.task('default', ['sass', 'watch']);