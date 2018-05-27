<?php
defined("IN_XYS") || die("Access Denied");
ob_end_clean();
ob_start();
@header("Expires: -1");
@header("Cache-Control: no-store, private, post-check=0, pre-check=0, max-age=0", FALSE);
@header("Pragma: no-cache");
@header("Content-type: text/plain; charset=utf-8");
?>
<div class="modal fade" id="modal-<?php echo $handlekey; ?>" tabindex="-1" role="dialog" aria-labelledby="modal-<?php echo $handlekey; ?>-label" aria-hidden="true">
<div class="modal-dialog" role="document">
<div class="modal-content">
<div class="modal-header">
    <h5 class="modal-title" id="modal-<?php echo $handlekey; ?>-label">提示信息</h5>
    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
</div>
<div class="modal-body">
<?php echo $message; ?>
</div>
<div class="modal-footer">
    <button type="button" class="btn btn-<?php echo $type; ?>" data-dismiss="modal">关闭</button>
</div>
</div>
</div>
</div>
<?php
$s = ob_get_contents();
ob_end_clean();
$s = preg_replace("/([\\x01-\\x08\\x0b-\\x0c\\x0e-\\x1f])+/", ' ', $s);
$s = str_replace(array(chr(0), ']]>'), array(' ', ']]&gt;'), $s);
if(function_exists("rewrite_url")) $s = rewrite_url($s);
echo $s;
exit;