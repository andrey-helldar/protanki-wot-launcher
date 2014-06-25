<?php

/**
 * @author Andrey Helldar <helldar@ai-rus.com>
 * @copyright (c) 2014, Andrey Helldar
 * @link http://ai-rus.com "AI RUS - Professional IT support"
 * @version 1.0
 */
if ($_SERVER['REMOTE_ADDR'] != "95.189.107.174" &&
	substr_count($_SERVER['REMOTE_ADDR'], "192.168.") == 0)
    die("FAIL");