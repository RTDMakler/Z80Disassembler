ScreenSize equ &4000

org &8000
	ld a, %00001111
FillAgain
	ld hl, &C000
	ld de, &C000+1
	ld bc, ScreenSize-1
	ld(hl),a
	ldir
	dec a
	cp 255
	jp nz, FillAgain
ret