var showEditForm = null;

function InitalizejQuery(s, e) {
    $('.draggable').draggable({
        helper: function (ev, ui) {
            return $(ev.target)
				.clone()
				.css("z-index", 100);
        }
    });
    $('.droppable').droppable(
                {
                    activeClass: "hover",
                    hoverClass: "dropTargetHover",
                    drop: function (ev, ui) {
                        // make a clone of the dragged item
                        var hitTestResult = scheduler.CalcHitTest(ev);
                        var interval = scheduler.GetCellInterval(hitTestResult.cell);
                        var clone = (ui.draggable).clone();
                        // get the row index:
                        row = $(clone).find("input[type='hidden']").val();
                        hf.Set('row', row);
                        hf.Set('intervalStart', interval.GetStart());
                        hf.Set('intervalEnd', interval.GetEnd());
                        
                        scheduler.PerformCallback("CreateAppointment");
                    }
                }
              );
}



