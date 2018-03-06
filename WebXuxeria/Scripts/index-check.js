'use strict'

$(function () {
    $(".sortable").sortable({
        //placeholder: "word-placeholder",
        deactivate: function (event, ui) { isOrdered(this); }
    });
    $(".sortable").disableSelection();
});

function isOrdered(word) {
    var todo = [];
    var lis = $(word).closest('.wordContainer').find('li');
    lis.each(function (index) {
        if (this.nodeName == "LI")
            todo.unshift($(this).data('partid')); //push() adds to the bottom. unshift() adds to the front
    });

    var sorted = _.sortBy(todo, function (num) {
        return num;
    });

    if(todo.reverse().equals(sorted)) {
        //play some sound

        lis.css('transform', '');
        //lis.css('border-radius', '0px');
        lis.css('border', 'gray solid 1px');
 
        $(word).closest('.wordContainer').find('ul').sortable("destroy"); //not sortable anymore
        $(word).closest('.wordContainer').find('.v').show(600,'linear'); //makes it visible
        //$(word).closest('.wordContainer').find('.v').addClass('shake shake-slow shake-constant');

        playFullWord($(word));
    }

}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Warn if overriding existing method
if (Array.prototype.equals)
    console.warn("Overriding existing Array.prototype.equals. Possible causes: New API defines the method, there's a framework conflict or you've got double inclusions in your code.");
// attach the .equals method to Array's prototype to call it on any array
Array.prototype.equals = function (array) {
    // if the other array is a falsy value, return
    if (!array)
        return false;

    // compare lengths - can save a lot of time 
    if (this.length != array.length)
        return false;

    for (var i = 0, l = this.length; i < l; i++) {
        // Check if we have nested arrays
        if (this[i] instanceof Array && array[i] instanceof Array) {
            // recurse into the nested arrays
            if (!this[i].equals(array[i]))
                return false;
        }
        else if (this[i] != array[i]) {
            // Warning - two different object instances will never be equal: {x:20} != {x:20}
            return false;
        }
    }
    return true;
}
// Hide method from for-in loops
Object.defineProperty(Array.prototype, "equals", { enumerable: false });