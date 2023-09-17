import { Component } from '@angular/core';
import { ApartamentoService } from './app.service';
import { take } from 'rxjs';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
})
export class AppComponent {
    public quadra?: string;

    public academia: boolean = false;
    public escola: boolean = false;
    public mercado: boolean = false;

    constructor(private apartamentoService: ApartamentoService) { }

    public buscarMelhorApartamento(): void {
        let estabelecimentos: string[] = [];

        if (this.academia) {
            estabelecimentos.push('ACADEMIA');
        }

        if (this.escola) {
            estabelecimentos.push('ESCOLA');
        }

        if (this.mercado) {
            estabelecimentos.push('MERCADO');
        }

        this.apartamentoService
            .getMelhorApartamento(estabelecimentos)
            .pipe(take(1))
            .subscribe((apartamento) => {
                this.quadra = apartamento.quadra;
            })
    }

    public setEstabelecimento(estabelecimento: number, marcado: boolean): void {
        if (estabelecimento === 1) {
            this.academia = marcado;
        }

        if (estabelecimento === 2) {
            this.escola = marcado;
        }

        if (estabelecimento === 3) {
            this.mercado = marcado;
        }
    }
}
