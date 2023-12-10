org &8400
	di
		ld a,&C3
		ld (&0038),a
		ld hl, InterruptHandler
		ld (&0039),hl
	ei
InfLoop:
	halt
jp InfLoop
InterruptHandler:
	exx
	ex af,af'
	ld hl,RasterColors:IH_RasterColor_Plus2
	ld	b,&f5
	in	a,(c)
	rra
	jp nc,InterruptHandlerOk
	ld hl,RasterColors
InterruptHandlerOk:
	ld bc,&7f00
	out (c),c
	ld a,(hl)
	out (c),a
	inc hl
	ld (IH_RasterColor_Plus2-2),hl
	ex af,af'
	exx
	ei
ret
RasterColors:
	db &4C,&43,&52,&5C,&5E,&5F