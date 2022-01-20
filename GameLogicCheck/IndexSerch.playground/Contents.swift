import UIKit

let indexSize = 4

var xx = 0
var yy = 0
var ii = 0

//case <L>
for y in 0...indexSize-1 {
    yy = y
    for x in 0...indexSize-1 { //0...3
        xx = indexSize-1 - x //3...0
        if(xx != 0){
            for i in 0...xx-1 {
                ii = i
                print("[\(ii+1),\(yy)] 에서 [\(ii),\(yy)]")
            }
        }
    }
}
