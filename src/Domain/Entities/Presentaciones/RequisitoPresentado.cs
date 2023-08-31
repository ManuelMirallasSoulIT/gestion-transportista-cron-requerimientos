using GDT.Common.Domain.Entities;
using System;

namespace Domain.Entities.Presentaciones;

public class RequisitoPresentado
{
	public long Id { get; set; }

	public Proveedores Proveedor { get; set; }

	public Unidades Unidad { get; set; }

	public Choferes Chofer { get; set; }

	public Requisitos Requisito { get; set; }

	public DateTime? Vencimiento { get; set; }

	public string Documento { get; set; }

	public string Status { get; set; }

	public DateTime? FechaPresentacion { get; set; }

	public DateTime? FechaAprobacion { get; set; }

	public string UsuarioAprobacion { get; set; }

	public string MotivoRechazo { get; set; }

	public RequisitoPresentado()
	{
		Requisito = new Requisitos();
	}

}